using ManagamentPias.Domain.Common;

namespace ManagamentPias.Domain.Entities;

public class Portfolio : AuditableBaseEntity
{
    public Portfolio(string description)
    {
        Description = description;
    }
    public string Description { get; set; } = null!;
}
