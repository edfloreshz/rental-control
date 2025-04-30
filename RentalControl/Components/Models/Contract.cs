namespace RentalControl.Components.Models;

public class Contract
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Address { get; set; } = string.Empty;
    public double Deposit { get; set; }
    public double Rent { get; set; }
    public string Business { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ContractStatus Status { get; set; }
    public ContractType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Tenant Tenant { get; set; } = new();
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