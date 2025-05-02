using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace RentalControl.Entities;

[Table("Tenants")]
public class Tenant : BaseModel
{
    [PrimaryKey] public Guid Id { get; set; }
    [Column] public string Name { get; set; } = string.Empty;
    [Column] public string Email { get; set; } = string.Empty;
    [Column] public string Phone { get; set; } = string.Empty;
    [Column] public Guid AddressId { get; set; }
    [Column] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Reference(typeof(Address))] public Address Address { get; set; } = new();
}