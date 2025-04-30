using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace RentalControl.Entities;

[Table("ContractGuarantors")]
public class ContractGuarantor : BaseModel
{
    [PrimaryKey] public Guid ContractId { get; set; }
    [PrimaryKey] public Guid GuarantorId { get; set; }
}