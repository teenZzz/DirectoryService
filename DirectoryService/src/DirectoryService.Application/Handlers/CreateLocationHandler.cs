using CSharpFunctionalExtensions;
using DirectoryService.Application.Repositories;
using DirectoryService.Contracts;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.Application;

public class CreateLocationHandler
{
    private readonly ILocationRepository _locationRepository;
    
    public CreateLocationHandler(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }
    
    /// <summary>
    /// Метод создает локацию.
    /// </summary>
    public async Task<Result<Guid, Errors>> Handle(CreateLocationRequest request, CancellationToken cancellationToken)
    {
        // Валидация входных параметров
        
        // Бизнес валидация
        
        // Создание доменных моделей
        var nameResult = Name.Create(request.Name);

        if (nameResult.IsFailure)
            return nameResult.Error.ToErrors();

        var addressResult = Address.Create(
            request.Address.Country, 
            request.Address.City, 
            request.Address.Street, 
            request.Address.HouseNumber, 
            request.Address.OfficeNumber, 
            request.Address.AdditionalInfo);

        if (addressResult.IsFailure)
            return addressResult.Error.ToErrors();

        var timezoneResult = Timezone.Create(request.Timezone);

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
        
        return location.Id;
    }
}