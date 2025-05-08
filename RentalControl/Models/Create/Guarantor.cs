namespace RentalControl.Models.Create;

public record Guarantor(
    Guid AddressId,
    string Name,
    string Phone,
    string Email
);