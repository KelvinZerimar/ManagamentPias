using Asp.Versioning;
using ManagementPias.App.Features.Notes.Commands.CreateNote;
using ManagementPias.App.Features.Notes.Commands.DeleteNote;
using ManagementPias.App.Features.Notes.Commands.UpdateNote;
using ManagementPias.App.Features.Notes.Queries.GetNoteById;
using ManagementPias.App.Features.Notes.Queries.GetNotes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace ManagementPias.WebApi.Controllers.v1;

[ApiVersion("1.0")]
public class NotesController(ILogger<AssetsController> logger, HybridCache hybridCache) : BaseApiController
{
    /// <summary>
    /// Gets a list of Notes 
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult> Get([FromQuery] GetNotesQuery filter, CancellationToken ct)
    {
        logger.LogInformation(message: "Request received to GetNotes.");
        var result = await hybridCache.GetOrCreateAsync("notes", async ct =>
        {
            return await Mediator!.Send(filter, ct);
        },
        tags: ["MyNotes"],
        cancellationToken: ct);
        logger.LogInformation(message: "Notes retrieved successfully.");
        return Ok(result);
        //return Ok(await Mediator!.Send(filter));
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult> Get(Guid id, [FromQuery] string partitionKey)
    {
        return Ok(await Mediator!.Send(new GetNoteByIdQuery { Id = id, PartitionKey = partitionKey }));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    public async Task<IActionResult> Post(CreateNoteCommand command)
    {
        var resp = await Mediator!.Send(command);
        return CreatedAtAction(nameof(Post), resp);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put(UpdateNoteCommand command)
    {
        var resp = await Mediator!.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id, [FromQuery] string partitionKey)
    {
        var command = new DeleteNoteCommand { Id = id, PartitionKey = partitionKey };
        var resp = await Mediator!.Send(command);
        return NoContent();
    }
}
