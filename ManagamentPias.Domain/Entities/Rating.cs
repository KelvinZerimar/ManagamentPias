using ManagamentPias.Domain.Common;
using ManagamentPias.Domain.Enums;

namespace ManagamentPias.Domain.Entities;

public class Rating : AuditableBaseEntity
{
    public decimal Valuation { get; set; }
    public DateTime DateSituation { get; set; }
    public Portafolio Portfolio { get; set; }
}
