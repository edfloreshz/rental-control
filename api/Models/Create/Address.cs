namespace RentalControl.Models.Create;

public record Address(
    string Street,
    string Number,
    string Neighborhood,
    string City,
    string State,
    string ZipCode,
    string Country
);