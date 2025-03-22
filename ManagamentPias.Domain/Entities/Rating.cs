using ManagementPias.Domain.Common;
using ManagementPias.Domain.Enums;

namespace ManagementPias.Domain.Entities;

public class Rating : AuditableBaseEntity
{
    public decimal Valuation { get; set; }
    public DateTime DateSituation { get; set; }
    public Portafolio Portfolio { get; set; }
}
