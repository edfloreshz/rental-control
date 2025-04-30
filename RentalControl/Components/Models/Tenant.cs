namespace RentalControl.Components.Models;

public class Tenant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Guid AddressId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Selected { get; set; }
    
    public Address Address { get; set; } = new();
    public List<Contract> Contracts { get; set; } = [];
}