using Asp.Versioning;
using ManagementPias.App.Features.Assets.Commands.CreateAsset;
using ManagementPias.App.Features.Assets.Commands.CreateRangeAssets;
using ManagementPias.App.Features.Assets.Queries.GetAssets;
using ManagementPias.App.Features.Assets.Queries.GetAssetsGroupedByDateSituation;
using ManagementPias.App.Features.Assets.Queries.GetCurrentAssets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace ManagementPias.WebApi.Controllers.v1;

[ApiVersion("1.0")]
public class AssetsController(ILogger<AssetsController> logger, HybridCache hybridCache) : BaseApiController
{
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

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(CreateAssetCommand command)
    {
        logger.LogInformation(message: "Request received to create Asset.");
        var resp = await Mediator!.Send(command);
        logger.LogInformation(message: "Asset created successfully.");
        return CreatedAtAction(nameof(Post), resp);
    }

    [HttpPost("bulk-insert")]
    // [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BulkInsert(CreateRangeAssetCommand command)
    {
        var resp = await Mediator!.Send(command);
        return CreatedAtAction(nameof(Post), resp);
    }

    [HttpGet("current-assets")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCurrentAssets(CancellationToken ct)
    {
        logger.LogInformation(message: "Request received to GetCurrentAssets.");
        var result = await hybridCache.GetOrCreateAsync("current-assets", async ct =>
        {
            var query = new GetCurrentAssetsQuery();
            return await Mediator!.Send(query, ct);
        },
         tags: ["piasAxa"],
         cancellationToken: ct);

        if (result == null)
            return NotFound();

        //var result = await Mediator!.Send(new GetCurrentAssetsQuery(), ct);

        logger.LogInformation(message: "GetCurrentAssets successfully.");
        return Ok(result);
    }

    [HttpGet("assets-by-date-situation")]
    public async Task<IActionResult> GetAssetGroupedByDateSituationAsync(CancellationToken ct)
    {
        logger.LogInformation(message: "Request received to GetAssetGroupedByDateSituationAsync.");

        var resp = await hybridCache.GetOrCreateAsync("assets-by-date-situation", async ct =>
        {
            var query = new GetAssetsGroupedByDateSituationQuery();
            return await Mediator!.Send(query, ct);
        },
         tags: ["piasAxa"],
         cancellationToken: ct);

        if (resp == null)
            return NotFound();
        //var resp = await Mediator!.Send(new GetAssetsGroupedByDateSituationQuery());

        logger.LogInformation(message: "GetAssetGroupedByDateSituationAsync successfully.");
        return Ok(resp);
    }

}
