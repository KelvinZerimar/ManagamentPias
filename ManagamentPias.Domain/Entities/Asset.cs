using ManagamentPias.Domain.Common;

namespace ManagamentPias.Domain.Entities;

public class Asset : AuditableBaseEntity
{
    public decimal ValuePatrimony { get; set; }
    public decimal NumUnit { get; set; }

    //public Guid PortfolioId { get; set; }
    public Guid RatingId { get; set; }
    //public Portfolio Portfolio { get; set; } = null!;
    public Rating Rating { get; set; } = null!;
}
