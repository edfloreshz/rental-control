namespace RentalControl.Components.Models;

public class ContractGuarantor
{
    public Guid ContractId { get; set; }
    public Guid GuarantorId { get; set; }

    public Contract Contract { get; set; } = new();
    public Guarantor Guarantor { get; set; } = new();
}