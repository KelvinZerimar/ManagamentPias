namespace ManagamentPias.App.Features.Assets.Queries.GetAssets;

public record AssetDetailsDto
{
    public string Description { get; init; } = null!;
    public decimal ValuePatrimony { get; set; }
    public decimal NumUnit { get; set; }
    public decimal ValuationRating { get; set; }
}
