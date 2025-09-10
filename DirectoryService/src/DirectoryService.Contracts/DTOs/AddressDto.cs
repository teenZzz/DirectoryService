namespace DirectoryService.Contracts.DTO;

public record AddressDto
{
    public required string Country { get; init; }
    
    public required string City { get; init; }
    
    public required string Street { get; init; }
    
    public required string HouseNumber { get; init; }
    
    public string? OfficeNumber { get; init; }

    public string? AdditionalInfo { get; init; }
}