namespace RentalControl.Models.Update;

public record Guarantor(
    Guid Id,
    Guid AddressId,
    string Name,
    string Phone,
    string Email
);