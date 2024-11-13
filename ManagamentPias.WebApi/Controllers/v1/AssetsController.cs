using Asp.Versioning;
using ManagamentPias.App.Features.Assets.Queries.GetAssets;
using Microsoft.AspNetCore.Mvc;

namespace ManagamentPias.WebApi.Controllers.v1;

[ApiVersion("1.0")]
public class AssetsController : BaseApiController
{
    public AssetsController()
    {

    }

    /// <summary>
    /// Gets a list of Assets 
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] GetAssetsQuery filter)
    {
        return Ok(await Mediator!.Send(filter));
    }
}
