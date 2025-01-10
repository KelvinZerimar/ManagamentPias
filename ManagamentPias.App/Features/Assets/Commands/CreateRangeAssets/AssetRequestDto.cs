namespace ManagamentPias.App.Features.Assets.Commands.CreateRangeAssets;

public class AssetRequestDto
{
    public DateTime Date { get; init; }
    public AssetDto[] LstAssets { get; set; } = null!;

}
public class AssetDto
{
    public decimal portfolioId { get; set; }
    public decimal NumUnit { get; set; }
    public decimal Valuation { get; set; }//ValuationRating
    public bool IsEdit { get; set; }
}
