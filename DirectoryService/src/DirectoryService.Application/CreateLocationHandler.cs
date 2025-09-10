using CSharpFunctionalExtensions;
using DirectoryService.Application.Repositories;
using DirectoryService.Contracts;
using DirectoryService.Domain.Entities;
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
    public async Task<Result<Guid>> Handle(CreateLocationRequest request, CancellationToken cancellationToken)
    {
        // Валидация входных параметров
        
        // Бизнес валидация
        
        // Создание доменных моделей
        var nameResult = Name.Create(request.Name);
        
        if (nameResult.IsFailure)
            return Result.Failure<Guid>(nameResult.Error);

        var addressResult = Address.Create(
            request.Address.Country, 
            request.Address.City, 
            request.Address.Street, 
            request.Address.HouseNumber, 
            request.Address.OfficeNumber, 
            request.Address.AdditionalInfo);
        
        if (addressResult.IsFailure)
            return Result.Failure<Guid>(addressResult.Error);

        var timezoneResult = Timezone.Create(request.Timezone);
        
        if (timezoneResult.IsFailure)
            return Result.Failure<Guid>(timezoneResult.Error);

        var name = nameResult.Value;
        var address = addressResult.Value;
        var timezone = timezoneResult.Value;

        var locationResult = Location.Create(name, address, timezone, true);

        if (locationResult.IsFailure)
            return Result.Failure<Guid>(locationResult.Error);

        var location = locationResult.Value;

        // Сохранение доменных моделей в БД
        await _locationRepository.Add(location, cancellationToken);
        
        return location.Id;
    }
}