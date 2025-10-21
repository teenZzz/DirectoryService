using System.Data;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Locations;
using DirectoryService.Application.Validation;
using DirectoryService.Contracts;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Departments;

// Command
public record CreateDepartmentCommand(CreateDepartmentRequest Request) : ICommand;

// Validator
public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .WithError(GeneralErrors.General.ValueIsRequired("request"));

        RuleFor(x => x.Request.Name)
            .MustBeValueObject(Name.Create);

        RuleFor(x => x.Request.Identifier)
            .MustBeValueObject(Identifier.Create);

        RuleFor(x => x.Request.ParentId)
            .Must(id => id == null || id.Value != Guid.Empty)
            .WithError(GeneralErrors.General.ValueIsInvalid("parent id"));

        RuleFor(x => x.Request.LocationIds)
            .NotNull()
            .WithError(GeneralErrors.General.ValueIsInvalid("locations"));
        
        RuleFor(x => x.Request.LocationIds)
            .Must(list => list is { Count: > 0 })
            .WithError(Error.Validation("department.location", "Department locations must contain at least one location"));
        
        RuleForEach(x => x.Request.LocationIds)
            .Must(id => id != Guid.Empty)
            .WithError(GeneralErrors.General.ValueIsInvalid("location id"));
    }
}

// Handler
public class CreateDepartmentHandler : ICommandHandler<Guid, CreateDepartmentCommand>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILogger<CreateDepartmentHandler> _logger;
    private readonly IValidator<CreateDepartmentCommand> _validator;
    private readonly ILocationRepository _locationRepository;
    
    public CreateDepartmentHandler(IDepartmentRepository departmentRepository, ILogger<CreateDepartmentHandler> logger, IValidator<CreateDepartmentCommand> validator, ILocationRepository locationRepository)
    {
        _departmentRepository = departmentRepository;
        _logger = logger;
        _validator = validator;
        _locationRepository = locationRepository;
    }
    
    public async Task<Result<Guid, Errors>> Handle(CreateDepartmentCommand command, CancellationToken cancellationToken)
    {
        // Валидация входных параметров
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToList();
        
        // Создание доменных моделей
        var id = Guid.NewGuid();
        var name = Name.Create(command.Request.Name).Value;
        
        var identifier = Identifier.Create(command.Request.Identifier).Value;
        
        // Проверка существования identifier
        var existsByIdentifier = await _departmentRepository.ExistsByIdentifierAsync(identifier, cancellationToken);

        if (existsByIdentifier.IsFailure)
            return GeneralErrors.General.Failure().ToErrors();
        
        if (existsByIdentifier.Value == true)
            return Error.Conflict(null, "A department with this identifier already exists.").ToErrors();

        Department? parent = null;

        // Получение родителя
        if (command.Request.ParentId != null)
        {
            var parentResult = await _departmentRepository.GetById(command.Request.ParentId.Value, cancellationToken);
            if (parentResult.IsFailure)
                return parentResult.Error;

            parent = parentResult.Value;
        }

        var locationIds = command.Request.LocationIds.ToList();
        
        // Проверка, что каждая локация существует
        var allLocationsExistResult = await _locationRepository.AllExistAsync(locationIds, cancellationToken);
        if (allLocationsExistResult.IsFailure)
            return allLocationsExistResult.Error;
        
        if (!allLocationsExistResult.Value)
            return Error.NotFound("locations.not.found", "Одна или несколько локаций не найдены").ToErrors();

        // Создание DepartmentLocations
        var departmentLocations = command.Request.LocationIds.Select(li => DepartmentLocation.Create(id, li).Value).ToList();
        
        // Создание Department
        var department = parent is null
            ? Department.CreateParent(id, name, identifier, departmentLocations)
            : Department.CreateChild(id, name, identifier, parent, departmentLocations);

        if (department.IsFailure)
            return department.Error.ToErrors();

        await _departmentRepository.AddAsync(department.Value, cancellationToken);
        
        _logger.LogInformation("Created department with id {id}", department.Value.Id);

        return department.Value.Id;
    }
}