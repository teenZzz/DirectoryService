using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Departments;
using DirectoryService.Application.Validation;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Requests;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Positions;

// Command
public record CreatePositionCommand(CreatePositionRequest Request) : ICommand;

// Validator
public class CreatePositionCommandValidator : AbstractValidator<CreatePositionCommand>
{
    public CreatePositionCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .WithError(GeneralErrors.General.ValueIsRequired("request"));

        RuleFor(x => x.Request.Name)
            .MustBeValueObject(Name.Create);

        RuleFor(x => x.Request.Description)
            .Must(description => description == null || description.Length <= 1000)
            .WithError(GeneralErrors.General.ValueIsInvalid("description"));
        
        RuleFor(x => x.Request.DepartmentIds)
            .NotNull()
            .WithError(GeneralErrors.General.ValueIsInvalid("departments"));
        
        RuleFor(x => x.Request.DepartmentIds)
            .NotNull()
            .Must(list => list is { Count: > 0 })
            .WithError(Error.Validation("department.positions", "Department positions must contain at least one department"));
        
        RuleForEach(x => x.Request.DepartmentIds)
            .Must(id => id != Guid.Empty)
            .WithError(GeneralErrors.General.ValueIsInvalid("department id"));

        RuleFor(x => x.Request.DepartmentIds)
            .Must(list => list == null || list.Distinct().Count() == list.Count)
            .WithError(Error.Validation("department.positions.duplicate", "Department positions must not contain duplicate department IDs"));
    }
}

// Handler
public class CreatePositionHandler : ICommandHandler<Guid, CreatePositionCommand>
{
    private readonly ILogger<CreatePositionHandler> _logger;
    private readonly IPositionRepository _positionRepository;
    private readonly IValidator<CreatePositionCommand> _validator;
    private readonly IDepartmentRepository _departmentRepository;
    
    public CreatePositionHandler(ILogger<CreatePositionHandler> logger, IPositionRepository positionRepository, IValidator<CreatePositionCommand> validator, IDepartmentRepository departmentRepository)
    {
        _logger = logger;
        _positionRepository = positionRepository;
        _validator = validator;
        _departmentRepository = departmentRepository;
    }

    public async Task<Result<Guid, Errors>> Handle(CreatePositionCommand command, CancellationToken cancellationToken)
    {
        // Валидация входных параметров
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToList();

        // Создание доменных моделей
        var id = Guid.NewGuid();
        var name = Name.Create(command.Request.Name).Value;
        string? description = command.Request.Description;
        
        // Проверка, что name не существует
        var nameExistsAndActive = await _positionRepository.NameExistAndActiveAsync(name.Value, cancellationToken);

        if (nameExistsAndActive.IsFailure)
            return nameExistsAndActive.Error.ToErrors();

        if (nameExistsAndActive.Value)
            return Error.Conflict("positions.name.exists", "Position with this name already exists.").ToErrors();
        
        // Проверка, что каждый департамент существует и активен
        var departmentIds = command.Request.DepartmentIds.ToList();

        var allDepartmentsExistResult = await _departmentRepository.AllExistAndActiveAsync(departmentIds, cancellationToken);
        if (allDepartmentsExistResult.IsFailure)
            return allDepartmentsExistResult.Error;
        
        if (!allDepartmentsExistResult.Value)
            return Error.NotFound("departments.not.found", "Departments not found.").ToErrors();

        // Создание DepartmentPositions
        var departmentPositions = command.Request.DepartmentIds.Select(di => DepartmentPosition.Create(di, id).Value).ToList();

        // Создание Positions
        var position = Position.Create(id, name, description, departmentPositions);

        if (position.IsFailure)
            return position.Error.ToErrors();

        var addResult = await _positionRepository.AddAsync(position.Value, cancellationToken);
        if (addResult.IsFailure)
            return addResult.Error.ToErrors();
        
        _logger.LogInformation("Created positions with id {id}", position.Value.Id);
        
        return position.Value.Id;
    }
}