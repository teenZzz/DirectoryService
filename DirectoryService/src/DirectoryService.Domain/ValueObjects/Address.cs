using CSharpFunctionalExtensions;

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

    public static Result<Address> Create(string country, string city, string street, string houseNumber, string? officeNumber, string? additionalInfo)
    {
        if (string.IsNullOrWhiteSpace(country) || !char.IsUpper(country.First()))
            return Result.Failure<Address>("Country cannot be empty and should be start with uppercase letter!");

        if (string.IsNullOrWhiteSpace(city) || !char.IsUpper(city.First()))
            return Result.Failure<Address>("City cannot be empty and should be start with uppercase letter!");
        
        if (string.IsNullOrWhiteSpace(street) || !char.IsUpper(street.First()))
            return Result.Failure<Address>("Street cannot be empty and should be start with uppercase letter!");

        if (string.IsNullOrWhiteSpace(houseNumber))
            return Result.Failure<Address>("House number cannot be empty!");

        if (officeNumber == null)
            return Result.Failure<Address>("Office number cannot be null!");

        if (additionalInfo == null)
            return Result.Failure<Address>("Additional info cannot be null!");

        var obj = new Address(country, city, street, houseNumber, officeNumber, additionalInfo);
        return Result.Success(obj);
    }
}