using ManagamentPias.App.Features.Assets.Queries.GetAssets;
using ManagamentPias.App.Interfaces.Repositories;
using ManagamentPias.App.Wrappers;
using ManagamentPias.Domain.Enums;
using MediatR;

namespace ManagamentPias.App.Features.Assets.Queries.GetCurrentAssets;

public class GetCurrentAssetsQuery : IRequest<Response<IEnumerable<AssetDetailsDto>>>
{
    public class GetCurrentAssetsQueryHandler(IAssetRepositoryAsync assetRepository) : IRequestHandler<GetCurrentAssetsQuery, Response<IEnumerable<AssetDetailsDto>>>
    {
        public async Task<Response<IEnumerable<AssetDetailsDto>>> Handle(GetCurrentAssetsQuery request, CancellationToken cancellationToken)
        {
            var resultData = await assetRepository.GetAssetByDateSituationAsync();
            var sum = resultData.Sum(a => a.ValuePatrimony);
            var shapeData = resultData.Select(rd => new AssetDetailsDto()
            {
                Description = rd.Rating.Portfolio.GetDescription(),
                ValuePatrimony = rd.ValuePatrimony,
                NumUnit = rd.NumUnit,
                ValuationRating = rd.Rating.Valuation,
                Percentage = sum == 0 ? 0 : Math.Round((rd.ValuePatrimony / sum) * 100, 2)
            }).OrderByDescending(a => a.ValuePatrimony);

            return await Task.FromResult(new Response<IEnumerable<AssetDetailsDto>>(shapeData));
        }
    }
}
