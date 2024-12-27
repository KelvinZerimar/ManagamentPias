using ManagamentPias.Domain.Common;

namespace ManagamentPias.Domain.Entities;

public class Asset : AuditableBaseEntity
{
    public decimal ValuePatrimony
    {
        get => Math.Round(Rating.Valuation * NumUnit, 2);
    }

    public decimal NumUnit { get; set; }
    public Guid RatingId { get; set; }
    public Rating Rating { get; set; } = null!;
}
