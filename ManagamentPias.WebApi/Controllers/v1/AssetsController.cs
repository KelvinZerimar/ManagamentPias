using Asp.Versioning;
using ManagamentPias.App.Features.Assets.Commands.CreateAsset;
using ManagamentPias.App.Features.Assets.Queries.GetAssets;
using ManagamentPias.App.Features.Assets.Queries.GetCurrentAssets;
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

    ///// <summary>
    ///// Get Asset by Id
    ///// </summary>
    ///// <param name="id"></param>
    ///// <returns></returns>
    //[HttpGet("{id}")]
    //public async Task<ActionResult> Get(int id)
    //{
    //    return Ok(await Mediator!.Send(new GetAssetByIdQuery { Id = id }));
    //}

    [HttpPost]
    // [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(CreateAssetCommand command)
    {
        var resp = await Mediator!.Send(command);
        return CreatedAtAction(nameof(Post), resp);
    }

    //[HttpPut]
    //// [Authorize]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> Put(UpdateAssetCommand command)
    //{
    //    var resp = await Mediator!.Send(command);
    //    return NoContent();
    //}
    //[HttpDelete]
    //// [Authorize]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> Delete(DeleteAssetCommand command)
    //{
    //    var resp = await Mediator!.Send(command);
    //    return NoContent();
    //}

    [HttpGet("current-assets")]
    // [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCurrentAssets([FromQuery] GetCurrentAssetsQuery query)
    {
        return Ok(await Mediator!.Send(query));
    }
}
