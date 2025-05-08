using RentalControl.Entities;

namespace RentalControl.Models.Get;

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
    ContractType Type,
    DateTime CreatedAt,
    Tenant Tenant,
    Address Address,
    List<Guarantor> Guarantors
);