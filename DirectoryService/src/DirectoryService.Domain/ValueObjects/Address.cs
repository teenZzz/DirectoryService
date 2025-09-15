using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.ValueObjects;

public record Address
{
    private Address(string country, string city, string street, string houseNumber, string? officeNumber, string? additionalInfo)
    {
        Country = country;
        City = city;
        Street = street;
        HouseNumber = houseNumber;
        OfficeNumber = officeNumber;
        AdditionalInfo = additionalInfo;
    }

    public string Country { get; }

    public string City { get; }

    public string Street { get; }

    public string HouseNumber { get; }

    public string? OfficeNumber { get; }

    public string? AdditionalInfo { get; }

    public static Result<Address, Error> Create(string country, string city, string street, string houseNumber, string? officeNumber, string? additionalInfo)
    {
        if (string.IsNullOrWhiteSpace(country) || !char.IsUpper(country.First()))
            return Error.Validation(null, "Country cannot be empty and should be start with uppercase letter!");

        if (string.IsNullOrWhiteSpace(city) || !char.IsUpper(city.First()))
            return Error.Validation(null, "City cannot be empty and should be start with uppercase letter!");

        if (string.IsNullOrWhiteSpace(street) || !char.IsUpper(street.First()))
            return Error.Validation(null, "Street cannot be empty and should be start with uppercase letter!");

        if (string.IsNullOrWhiteSpace(houseNumber))
            return Error.Validation(null, "House number cannot be empty!");

        if (officeNumber == null)
            return Error.Validation(null, "Office number cannot be null!");

        if (additionalInfo == null)
            return Error.Validation(null, "Additional info cannot be null!");
        
        return new Address(country, city, street, houseNumber, officeNumber, additionalInfo);
    }
}