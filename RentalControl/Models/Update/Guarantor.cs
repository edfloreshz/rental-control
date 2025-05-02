namespace RentalControl.Models.Update;

public class Guarantor
{
    public Guid Id { get; set; }
    public Guid AddressId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Address Address { get; set; } = new();
}