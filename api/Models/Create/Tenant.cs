namespace RentalControl.Models.Create;

public record Tenant(
    Guid AddressId,
    string Name,
    string Email,
    string Phone
);