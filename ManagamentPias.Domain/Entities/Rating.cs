using ManagamentPias.Domain.Common;

namespace ManagamentPias.Domain.Entities;

public class Rating : AuditableBaseEntity
{
    public decimal Valuation { get; set; }
    public DateTime DateSituation { get; set; }

    public Guid PortfolioId { get; set; }
    public Portfolio Portfolio { get; set; } = null!;
}
