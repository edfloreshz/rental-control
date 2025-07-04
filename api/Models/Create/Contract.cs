using RentalControl.Entities;

namespace RentalControl.Models.Create;

public record Contract(
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