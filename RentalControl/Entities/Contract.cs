using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace RentalControl.Entities;

[Table("Contracts")]
public class Contract : BaseModel
{
    [PrimaryKey] public Guid Id { get; set; }
    [Column] public Guid TenantId { get; set; }
    [Column] public Guid AddressId { get; set; }
    [Column] public decimal Deposit { get; set; }
    [Column] public decimal Rent { get; set; }
    [Column] public string Business { get; set; } = string.Empty;
    [Column] public DateTime StartDate { get; set; }
    [Column] public DateTime EndDate { get; set; }
    [Column] public ContractStatus Status { get; set; }
    [Column] public ContractType Type { get; set; }
    [Column] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Reference(typeof(Tenant))] public Tenant Tenant { get; set; } = new();
    [Reference(typeof(Address))] public Address Address { get; set; } = new();
    [Reference(typeof(ContractGuarantor))] public List<Guarantor> Guarantors { get; set; } = [];
}

public enum ContractStatus
{
    Active,
    Expired,
    Terminated
}

public enum ContractType
{
    Yearly
}