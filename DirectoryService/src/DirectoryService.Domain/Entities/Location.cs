using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.Domain.Entities;

public class Location
{
    private Location(Guid id, Name name, Address address, Timezone timeZone, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Address = address;
        Timezone = timeZone;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }

    public Name Name { get; private set; }

    public Address Address { get; private set; }

    public Timezone Timezone { get; private set; }

    public bool IsActive { get; private set; } = true;

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }
}