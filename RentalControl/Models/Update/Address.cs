namespace RentalControl.Models.Update;

public record Address(
    Guid Id,
    string Street,
    string Number,
    string Neighborhood,
    string City,
    string State,
    string ZipCode,
    string Country
);