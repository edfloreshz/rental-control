using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace RentalControl.Entities;

[Table("tenants")]
public class Tenant : BaseModel
{
    [PrimaryKey("id")] public Guid Id { get; set; }
    [Column("name")] public string Name { get; set; } = string.Empty;
    [Column("email")] public string Email { get; set; } = string.Empty;
    [Column("phone")] public string Phone { get; set; } = string.Empty;
    [Column("address")] public string Address { get; set; } = string.Empty;
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}