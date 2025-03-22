using Asp.Versioning;
using ManagementPias.App.Features.Ratings.Queries.GetPortfolios;
using Microsoft.AspNetCore.Mvc;

namespace ManagementPias.WebApi.Controllers.v1;
[ApiVersion("1.0")]
public class RatingsController : BaseApiController
{

    public RatingsController()
    {

    }

    /// <summary>
    /// Gets a list of Portfolios 
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpGet("Portfolios")]
    public async Task<ActionResult> Get([FromQuery] GetPortfoliosQuery filter)
    {
        return Ok(await Mediator!.Send(filter));
    }
}
