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
        var name = Name.Create(request.Name).Value;

        var address = Address.Create(
            request.Address.Country, 
            request.Address.City, 
            request.Address.Street, 
            request.Address.HouseNumber, 
            request.Address.OfficeNumber, 
            request.Address.AdditionalInfo).Value;

        var timezone = Timezone.Create(request.Timezone).Value;

        var location = Location.Create(name, address, timezone, true);
        
        // Сохранение доменных моделей в БД
        await _locationRepository.Add(location.Value, cancellationToken);
        
        return location.Value.Id;
    }
}