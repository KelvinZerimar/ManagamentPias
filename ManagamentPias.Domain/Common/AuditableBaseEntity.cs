namespace ManagamentPias.Domain.Common;

public abstract class AuditableBaseEntity : BaseEntity
{
    public string CreatedBy { get; set; } = "kramirez";
    public DateTime Created { get; set; }
    public string? LastModifiedBy { get; init; }
    public DateTime? LastModified { get; set; }



}
