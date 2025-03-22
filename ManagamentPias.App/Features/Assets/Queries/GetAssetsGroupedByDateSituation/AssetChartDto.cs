using ManagementPias.Domain.Enums;

namespace ManagementPias.App.Features.Assets.Queries.GetAssetsGroupedByDateSituation;

public class AssetChartDto // PortfolioValuePatrimonyDto
{
    public required Portafolio Portfolio { get; set; }
    public required AssetValue[] Assets { get; set; }
}

public record AssetValue(
    int Year,
    int Month,
    decimal ValuePatrimony
    );

public record PortfolioValuePatrimonyDto(
    Portafolio Portafolio,
    Decimal[] ValuesPatrimonyByMonth
    );

public record responseHistoryPortfolio(
    string[] DateAll,
    PortfolioValuePatrimonyDto[] PortfolioValuePatrimonyDto
    );