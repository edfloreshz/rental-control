using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace RentalControl.Entities;

[Table("contracts")]
public class Contract : BaseModel
{
    [PrimaryKey("id")] public Guid Id { get; set; }
    [Column("tenant_id")] public Guid TenantId { get; set; }
    [Column("address")] public string Address { get; set; } = string.Empty;
    [Column("deposit")] public double Deposit { get; set; }
    [Column("rent")] public double Rent { get; set; }
    [Column("business")] public string Business { get; set; } = string.Empty;
    [Column("start_date")] public DateTime StartDate { get; set; }
    [Column("end_date")] public DateTime EndDate { get; set; }
    [Column("status")] public ContractStatus Status { get; set; }
    [Column("type")] public ContractType Type { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Reference(typeof(Tenant))] public Tenant Tenant { get; set; } = new();
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