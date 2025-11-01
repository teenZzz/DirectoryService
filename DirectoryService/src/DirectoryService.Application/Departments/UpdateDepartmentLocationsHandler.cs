using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Locations;
using DirectoryService.Application.Validation;
using DirectoryService.Contracts.Requests;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Departments;

public record UpdateDepartmentLocationsCommand(UpdateDepartmentLocationsRequest Request) : ICommand;

public class UpdateDepartmentLocationsCommandValidator : AbstractValidator<UpdateDepartmentLocationsCommand>
{
    public UpdateDepartmentLocationsCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .WithError(GeneralErrors.General.ValueIsRequired("request"));

        RuleFor(x => x.Request.DepartmentId)
            .Must(id => id != Guid.Empty)
            .WithError(GeneralErrors.General.ValueIsInvalid("department id"));
        
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

public class UpdateDepartmentLocationsHandler : ICommandHandler<Guid, UpdateDepartmentLocationsCommand>
{
    private readonly ILogger<UpdateDepartmentLocationsHandler> _logger;
    private readonly IValidator<UpdateDepartmentLocationsCommand> _validator;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILocationRepository _locationRepository;
    
    public UpdateDepartmentLocationsHandler(ILogger<UpdateDepartmentLocationsHandler> logger, IValidator<UpdateDepartmentLocationsCommand> validator, IDepartmentRepository departmentRepository, ILocationRepository locationRepository)
    {
        _logger = logger;
        _validator = validator;
        _departmentRepository = departmentRepository;
        _locationRepository = locationRepository;
    }
    
    public async Task<Result<Guid, Errors>> Handle(UpdateDepartmentLocationsCommand command, CancellationToken cancellationToken)
    {
        // Валидация входных параметров
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToList();

        // Получение департамента
        var departmentResult = await _departmentRepository.GetById(command.Request.DepartmentId, cancellationToken);
        if (departmentResult.IsFailure)
            return departmentResult.Error;

        var department = departmentResult.Value;
        
        // Проверка существования локаций
        var allLocationsExistAndActive = await _locationRepository.AllExistAndActiveAsync(command.Request.LocationIds, cancellationToken);
        if (allLocationsExistAndActive.IsFailure)
            return allLocationsExistAndActive.Error;

        if (allLocationsExistAndActive.Value == false)
            return Error.NotFound("locations.not.found", "One or more locations not found").ToErrors();

        // Создание новых локаций для департамента
        var departmentLocations = command.Request.LocationIds.Select(li => new DepartmentLocation(department.Id, li)).ToList();
        
        // Обновление локаций у департамента
        var updateResult = department.UpdateLocations(departmentLocations);
        if (updateResult.IsFailure)
            return updateResult.Error.ToErrors();

        var saveChanges = await _departmentRepository.SaveChangesAsync(cancellationToken);
        if (saveChanges.IsFailure)
            return saveChanges.Error.ToErrors();
        
        _logger.LogInformation("Update locations for department with id: {departmentId}", department.Id);

        return department.Id;
    }
}