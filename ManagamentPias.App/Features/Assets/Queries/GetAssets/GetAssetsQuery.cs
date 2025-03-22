using ManagementPias.App.Interfaces;
using ManagementPias.App.Interfaces.Repositories;
using ManagementPias.App.Parameters;
using ManagementPias.App.Wrappers;
using MediatR;

namespace ManagementPias.App.Features.Assets.Queries.GetAssets;

public class GetAssetsQuery : QueryParameter, IRequest<PagedResponse<IEnumerable<AssetDetailsDto>>>
{
    public string? Description { get; set; } = null;

    public class GetAssetsQueryHandler : IRequestHandler<GetAssetsQuery, PagedResponse<IEnumerable<AssetDetailsDto>>>
    {
        private readonly IAssetRepositoryAsync _assetRepository;
        private readonly IModelHelper _modelHelper;

        public GetAssetsQueryHandler(IAssetRepositoryAsync assetRepository, IModelHelper modelHelper)
        {
            _assetRepository = assetRepository;
            _modelHelper = modelHelper;
        }

        public async Task<PagedResponse<IEnumerable<AssetDetailsDto>>> Handle(GetAssetsQuery request, CancellationToken cancellationToken)
        {

            var validFilter = request;
            //filtered fields security
            if (!string.IsNullOrEmpty(validFilter.Fields))
            {
                //limit to fields in view model
                validFilter.Fields = _modelHelper.ValidateModelFields<GetAssetsViewModel>(validFilter.Fields);
            }
            if (string.IsNullOrEmpty(validFilter.Fields))
            {
                //default fields from view model
                validFilter.Fields = _modelHelper.GetModelFields<GetAssetsViewModel>();
            }
            // query based on filter
            var entityAssets = await _assetRepository.GetPagedAssetResponseAsync(validFilter);
            var data = entityAssets.data;
            RecordsCount recordCount = entityAssets.recordsCount;
            // response wrapper
            return new PagedResponse<IEnumerable<AssetDetailsDto>>(data, validFilter.PageNumber, validFilter.PageSize, recordCount);
        }
    }
}
