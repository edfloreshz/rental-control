namespace RentalControl.Models.Get;

public record Guarantor(
    Guid Id,
    Guid AddressId,
    string Name,
    string Phone,
    string Email,
    DateTime CreatedAt,
    Address Address
);