namespace ManagementPias.Domain.Common;

public abstract class BaseEntity
{
    public virtual int Id { get; set; }
    //public virtual Guid Id { get; set; } = Guid.NewGuid();
}
