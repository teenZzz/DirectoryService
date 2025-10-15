using CSharpFunctionalExtensions;
using DirectoryService.Application.Locations;
using DirectoryService.Application.Repositories;
using DirectoryService.Contracts;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application;

public class CreateLocationHandler
{
    private readonly ILocationRepository _locationRepository;
    private readonly ILogger<CreateLocationHandler> _logger;

    public CreateLocationHandler(ILocationRepository locationRepository, ILogger<CreateLocationHandler> logger)
    {
        _locationRepository = locationRepository;
        _logger = logger;
    }
    
    /// <summary>
    /// Метод создает локацию.
    /// </summary>
    public async Task<Result<Guid, Errors>> Handle(CreateLocationCommand command, CancellationToken cancellationToken)
    {
        // Валидация входных параметров
        
        // Бизнес валидация
        
        // Создание доменных моделей
        var nameResult = Name.Create(command.Request.Name);

        if (nameResult.IsFailure)
            return nameResult.Error.ToErrors();

        var addressResult = Address.Create(
            command.Request.Address.Country, 
            command.Request.Address.City, 
            command.Request.Address.Street, 
            command.Request.Address.HouseNumber, 
            command.Request.Address.OfficeNumber, 
            command.Request.Address.AdditionalInfo);

        if (addressResult.IsFailure)
            return addressResult.Error.ToErrors();

        var timezoneResult = Timezone.Create(command.Request.Timezone);

        if (timezoneResult.IsFailure)
            return timezoneResult.Error.ToErrors();

        var name = nameResult.Value;
        var address = addressResult.Value;
        var timezone = timezoneResult.Value;

        var locationResult = Location.Create(name, address, timezone, true);

        if (locationResult.IsFailure)
            return locationResult.Error.ToErrors();

        var location = locationResult.Value;

        // Сохранение доменных моделей в БД
        await _locationRepository.Add(location, cancellationToken);
        
        _logger.LogInformation("Created locations with id {id}", location.Id);
        
        return location.Id;
    }
}