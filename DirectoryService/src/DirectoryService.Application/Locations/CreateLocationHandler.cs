using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Validation;
using DirectoryService.Contracts;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Locations;

// Command DTO
public record CreateLocationCommand(CreateLocationRequest Request) : ICommand;

// Validator
public class CreateLocationCommandValidator : AbstractValidator<CreateLocationCommand>
{
    public CreateLocationCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .WithError(GeneralErrors.General.ValueIsRequired("request"));
        
        RuleFor(x => x.Request.Name)
            .MustBeValueObject(Name.Create);

        RuleFor(x => x.Request.Address)
            .MustBeValueObject(a => Address.Create(
                a.Country,
                a.City,
                a.Street,
                a.HouseNumber,
                a.OfficeNumber,
                a.AdditionalInfo));

        RuleFor(x => x.Request.Timezone)
            .MustBeValueObject(Timezone.Create);
    }
}

// Handler
public class CreateLocationHandler : ICommandHandler<Guid, CreateLocationCommand>
{
    private readonly ILocationRepository _locationRepository;
    private readonly ILogger<CreateLocationHandler> _logger;
    private readonly IValidator<CreateLocationCommand> _validator;

    public CreateLocationHandler(ILocationRepository locationRepository, ILogger<CreateLocationHandler> logger, IValidator<CreateLocationCommand> validator)
    {
        _locationRepository = locationRepository;
        _logger = logger;
        _validator = validator;
    }
    
    /// <summary>
    /// Метод создает локацию.
    /// </summary>
    public async Task<Result<Guid, Errors>> Handle(CreateLocationCommand command, CancellationToken cancellationToken)
    {
        // Валидация входных параметров
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToList();
        
        // Создание доменных моделей
        var name = Name.Create(command.Request.Name).Value;

        var address = Address.Create(
            command.Request.Address.Country, 
            command.Request.Address.City, 
            command.Request.Address.Street, 
            command.Request.Address.HouseNumber, 
            command.Request.Address.OfficeNumber, 
            command.Request.Address.AdditionalInfo).Value;
        
        var timezone = Timezone.Create(command.Request.Timezone).Value;
        
        // Проверка, что локация с таким именем еще не создана
        var existsByName = await _locationRepository.ExistsByNameAsync(name, cancellationToken);
        if (existsByName.Value == true)
            return Error.Conflict(null, "A location with this name already exists.").ToErrors();

        // Проверка, что локация с таким адресом еще не создана
        var existsByAddress = await _locationRepository.ExistsByAddressAsync(address, cancellationToken);

        if (existsByAddress.IsFailure)
            return GeneralErrors.General.Failure().ToErrors();
        
        if (existsByAddress.Value == true)
            return Error.Conflict(null, "A location with this address already exists.").ToErrors();

        var locationResult = Location.Create(name, address, timezone, true);
        if (locationResult.IsFailure)
            return locationResult.Error.ToErrors();

        var location = locationResult.Value;

        // Сохранение доменных моделей в БД
        var addResult = await _locationRepository.AddAsync(location, cancellationToken);
        if (addResult.IsFailure)
            return addResult.Error.ToErrors();
        
        _logger.LogInformation("Created locations with id {id}", location.Id);
        
        return location.Id;
    }
}