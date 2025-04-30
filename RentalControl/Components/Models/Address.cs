namespace RentalControl.Components.Models;

public class Address
{
    public Guid Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Selected { get; set; }

    public List<Tenant> Tenants { get; set; } = [];
    public List<Contract> Contracts { get; set; } = [];
    public List<Guarantor> Guarantors { get; set; } = [];
    
    public string ShortAddress => $"{Street} {Number}, {Neighborhood}";
    public string FullAddress => $"{Street} {Number}, {Neighborhood}, {City}, {State}, {ZipCode}, {Country}";
}