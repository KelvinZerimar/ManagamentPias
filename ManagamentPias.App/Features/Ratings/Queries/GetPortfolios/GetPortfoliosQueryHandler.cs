using ManagementPias.App.Wrappers;
using ManagementPias.Domain.Enums;
using MediatR;

namespace ManagementPias.App.Features.Ratings.Queries.GetPortfolios;

public class GetPortfoliosQuery : IRequest<Response<IEnumerable<PortfolioDto>>>
{
    public class GetPortafoliosQueryHandler : IRequestHandler<GetPortfoliosQuery, Response<IEnumerable<PortfolioDto>>>
    {
        public async Task<Response<IEnumerable<PortfolioDto>>> Handle(GetPortfoliosQuery request, CancellationToken cancellationToken)
        {
            var portafolios = Enum.GetValues(typeof(Portafolio)).Cast<Portafolio>();
            var response = portafolios.Select(p => new PortfolioDto
            {
                Id = (int)p,
                Name = p.GetDescription()
            });

            return await Task.FromResult(new Response<IEnumerable<PortfolioDto>>(response));
        }
    }
}
