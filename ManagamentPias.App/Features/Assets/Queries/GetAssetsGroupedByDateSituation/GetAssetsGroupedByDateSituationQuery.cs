using ManagementPias.App.Interfaces.Repositories;
using ManagementPias.App.Wrappers;
using MediatR;
using System.Globalization;

namespace ManagementPias.App.Features.Assets.Queries.GetAssetsGroupedByDateSituation;

public class GetAssetsGroupedByDateSituationQuery : IRequest<Response<responseHistoryPortfolio>>
{
    public class GetAssetsGroupedByDateSituationQueryHandler : IRequestHandler<GetAssetsGroupedByDateSituationQuery, Response<responseHistoryPortfolio>>
    {
        private readonly IAssetRepositoryAsync _assetRepository;

        public GetAssetsGroupedByDateSituationQueryHandler(IAssetRepositoryAsync assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<Response<responseHistoryPortfolio>> Handle(GetAssetsGroupedByDateSituationQuery request, CancellationToken cancellationToken)
        {
            var result = await _assetRepository.GetValuePatrimonyByPortfolioMonthlyAsync(2024);
            var assetChartDtoList = result.Select(group => new AssetChartDto
            {
                Portfolio = group.Portfolio,
                Assets = group.Assets.Select(asset => new AssetValue
                (
                    Year: asset.Year,
                    Month: asset.Month,
                    ValuePatrimony: asset.ValuePatrimony
                )).ToArray()
            });

            var resp = assetChartDtoList.Select(a => a.Assets.OrderBy(x => x.Year).ThenBy(x => x.Month));
            var dateAll = resp.First().Select(x => new[] { $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Month).Substring(0, 1)
                .ToUpper()}{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Month).Substring(1)} {x.Year}" })
                .ToArray();

            var response = assetChartDtoList.Select(a => new PortfolioValuePatrimonyDto(
                Portafolio: a.Portfolio,
                ValuesPatrimonyByMonth: a.Assets.OrderBy(x => x.Year).ThenBy(x => x.Month).Select(p => p.ValuePatrimony).ToArray()
                ));

            return new Response<responseHistoryPortfolio>(new responseHistoryPortfolio(
                DateAll: dateAll.SelectMany(x => x).ToArray(),
                PortfolioValuePatrimonyDto: response.ToArray()
                ));
        }
    }
}
