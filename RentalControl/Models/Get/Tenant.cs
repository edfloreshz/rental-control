namespace RentalControl.Models.Get;

public record Tenant(
    Guid Id,
    Guid AddressId,
    string Name,
    string Email,
    string Phone,
    DateTime CreatedAt,
    Address Address,
    List<Contract> Contracts
);