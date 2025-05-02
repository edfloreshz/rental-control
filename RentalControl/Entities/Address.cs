using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace RentalControl.Entities;

[Table("Addresses")]
public class Address : BaseModel
{
    [PrimaryKey] public Guid Id { get; set; }
    [Column] public string Street { get; set; } = string.Empty;
    [Column] public string Number { get; set; } = string.Empty;
    [Column] public string Neighborhood { get; set; } = string.Empty;
    [Column] public string City { get; set; } = string.Empty;
    [Column] public string State { get; set; } = string.Empty;
    [Column] public string ZipCode { get; set; } = string.Empty;
    [Column] public string Country { get; set; } = string.Empty;
    [Column] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}