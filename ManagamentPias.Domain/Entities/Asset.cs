using ManagementPias.Domain.Common;

namespace ManagementPias.Domain.Entities;

public class Asset : AuditableBaseEntity
{
    public decimal ValuePatrimony
    {
        get => Math.Round(Rating.Valuation * NumUnit, 2);
    }

    public decimal NumUnit { get; set; }
    public int RatingId { get; set; }
    public Rating Rating { get; set; } = null!;
}
