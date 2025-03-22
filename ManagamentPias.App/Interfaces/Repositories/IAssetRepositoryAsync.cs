using ManagementPias.App.Features.Assets.Queries.GetAssets;
using ManagementPias.App.Features.Assets.Queries.GetAssetsGroupedByDateSituation;
using ManagementPias.App.Parameters;
using ManagementPias.Domain.Entities;

namespace ManagementPias.App.Interfaces.Repositories;

public interface IAssetRepositoryAsync : IGenericRepositoryAsync<Asset>
{
    Task<(IEnumerable<AssetDetailsDto> data, RecordsCount recordsCount)> GetPagedAssetResponseAsync(GetAssetsQuery requestParameters);
    Task<IEnumerable<Asset>> GetAssetByDateSituationAsync();
    Task<IEnumerable<AssetChartDto>> GetValuePatrimonyByPortfolioMonthlyAsync(int year);
}
