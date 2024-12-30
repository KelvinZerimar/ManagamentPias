using AutoMapper;
using ManagamentPias.App.Interfaces.Repositories;
using ManagamentPias.App.Wrappers;
using ManagamentPias.Domain.Entities;
using MediatR;

namespace ManagamentPias.App.Features.Assets.Commands.CreateAsset;

public partial class CreateAssetCommand : Asset, IRequest<Response<Asset>>
{
}

public class CreateAssetCommandHandler(IAssetRepositoryAsync _repository,
    IMapper _mapper) : IRequestHandler<CreateAssetCommand, Response<Asset>>
{
    public async Task<Response<Asset>> Handle(CreateAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = _mapper.Map<Asset>(request);
        await _repository.AddAsync(asset);
        return new Response<Asset>(asset);
    }
}
