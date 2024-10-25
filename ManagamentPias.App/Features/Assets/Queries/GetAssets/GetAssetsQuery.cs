using ManagamentPias.App.Interfaces;
using ManagamentPias.App.Interfaces.Repositories;
using ManagamentPias.App.Parameters;
using ManagamentPias.App.Wrappers;
using ManagamentPias.Domain.Entities;
using MediatR;

namespace ManagamentPias.App.Features.Assets.Queries.GetAssets;

public class GetAssetsQuery : QueryParameter, IRequest<PagedResponse<IEnumerable<Entity>>>
{
    public string? Description { get; set; } = null;

    public class GetAssetsQueryHandler : IRequestHandler<GetAssetsQuery, PagedResponse<IEnumerable<Entity>>>
    {
        private readonly IAssetRepositoryAsync _assetRepository;
        private readonly IModelHelper _modelHelper;

        public GetAssetsQueryHandler(IAssetRepositoryAsync assetRepository, IModelHelper modelHelper)
        {
            _assetRepository = assetRepository;
            _modelHelper = modelHelper;
        }

        public async Task<PagedResponse<IEnumerable<Entity>>> Handle(GetAssetsQuery request, CancellationToken cancellationToken)
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
            var entityCustomers = await _assetRepository.GetPagedAssetReponseAsync(validFilter);
            var data = entityCustomers.data;
            RecordsCount recordCount = entityCustomers.recordsCount;
            // response wrapper
            return new PagedResponse<IEnumerable<Entity>>(data, validFilter.PageNumber, validFilter.PageSize, recordCount);
        }
    }
}
