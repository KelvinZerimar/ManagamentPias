using AutoMapper;
using ManagementPias.App.Interfaces.Repositories;
using ManagementPias.App.Wrappers;
using ManagementPias.Domain.Entities;
using ManagementPias.Domain.Enums;
using MediatR;

namespace ManagementPias.App.Features.Assets.Commands.CreateRangeAssets;

public partial class CreateRangeAssetCommand : AssetRequestDto, IRequest<Response<int>>
{
}
public class CreateRangeAssetCommandHandler(IAssetRepositoryAsync _repository,
    IMapper _mapper) : IRequestHandler<CreateRangeAssetCommand, Response<int>>

{
    public async Task<Response<int>> Handle(CreateRangeAssetCommand request, CancellationToken cancellationToken)
    {
        var requestData = _mapper.Map<AssetRequestDto>(request);
        var assets = requestData.LstAssets.Select(a => new Asset()
        {
            NumUnit = a.NumUnit,
            Created = DateTime.Now.ToUniversalTime(),
            Rating = new Rating()
            {
                DateSituation = requestData.Date.ToUniversalTime(),
                Portfolio = (Portafolio)a.portfolioId,
                Valuation = a.Valuation,
                Created = DateTime.Now.ToUniversalTime()
            },
        });
        await _repository.BulkInsertAsync(assets);
        return new Response<int>(assets.Count());
    }
}