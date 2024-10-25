using ManagamentPias.App.Features.Assets.Queries.GetAssets;
using ManagamentPias.App.Parameters;
using ManagamentPias.Domain.Entities;

namespace ManagamentPias.App.Interfaces.Repositories;

public interface IAssetRepositoryAsync : IGenericRepositoryAsync<Asset>
{
    Task<(IEnumerable<Entity> data, RecordsCount recordsCount)> GetPagedAssetReponseAsync(GetAssetsQuery requestParameters);
}
