using RentalControl.Entities;

namespace RentalControl.Models.Update;

public record Contract(
    Guid Id,
    Guid TenantId,
    Guid AddressId,
    decimal Deposit,
    decimal Rent,
    string Business,
    DateTime StartDate,
    DateTime EndDate,
    ContractStatus Status,
    ContractType Type
);