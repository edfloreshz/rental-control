namespace RentalControl.Models.Create;

public class Tenant
{
    public Guid AddressId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}