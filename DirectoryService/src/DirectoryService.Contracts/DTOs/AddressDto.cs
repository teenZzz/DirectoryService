namespace DirectoryService.Contracts.DTO;

public class AddressDto
{
    public required string Country { get; set; }
    
    public required string City { get; set; }
    
    public required string Street { get; set; }
    
    public required string HouseNumber { get; set; }
    
    public required string OfficeNumber { get; set; }

    public required string AdditionalInfo { get; set; }
}