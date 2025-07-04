namespace RentalControl.Models.Update;

public record Tenant(
    Guid Id,
    Guid AddressId,
    string Name,
    string Email,
    string Phone
);