using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.Domain.Entities;

public class Location
{
    // EF Core
    private Location()
    {
    }
    
    private List<DepartmentLocation> _departments;
    
    private Location(Name name, Address address, Timezone timeZone, bool isActive)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Name = name;
        Address = address;
        Timezone = timeZone;
        IsActive = isActive;
    }

    public Guid Id { get; private set; }

    public Name Name { get; private set; }

    public Address Address { get; private set; }

    public Timezone Timezone { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }
    
    public IReadOnlyList<DepartmentLocation> DepartmentLocations => _departments = [];
    
    public static Result<Location, Error> Create(Name name, Address address, Timezone timeZone, bool isActive)
    {
        return new Location(name, address, timeZone, isActive);
    }
}