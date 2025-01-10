using AutoMapper;
using ManagamentPias.App.Interfaces.Repositories;
using ManagamentPias.App.Wrappers;
using ManagamentPias.Domain.Entities;
using ManagamentPias.Domain.Enums;
using MediatR;

namespace ManagamentPias.App.Features.Assets.Commands.CreateRangeAssets;

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
            Created = DateTime.Now,
            Rating = new Rating()
            {
                DateSituation = requestData.Date,
                Portfolio = (Portafolio)a.portfolioId,
                Valuation = a.Valuation,
                Created = DateTime.Now
            },
        });
        await _repository.BulkInsertAsync(assets);
        return new Response<int>(assets.Count());
    }
}