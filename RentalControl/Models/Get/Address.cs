namespace RentalControl.Models.Get;

public record Address(
    Guid Id,
    string Street,
    string Number,
    string Neighborhood,
    string City,
    string State,
    string ZipCode,
    string Country,
    DateTime CreatedAt,
    List<Contract> Contracts
);