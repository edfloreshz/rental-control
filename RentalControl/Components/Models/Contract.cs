using RentalControl.Entities;

namespace RentalControl.Components.Models;

public class Contract
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid AddressId { get; set; }
    public decimal Deposit { get; set; }
    public decimal Rent { get; set; }
    public string Business { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ContractStatus Status { get; set; }
    public ContractType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Tenant Tenant { get; set; } = new();
    public Address Address { get; set; } = new();
    public List<Guarantor> Guarantors { get; set; } = [];
}