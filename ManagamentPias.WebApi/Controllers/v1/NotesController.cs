using Asp.Versioning;
using ManagamentPias.App.Features.Notes.Commands.CreateNote;
using ManagamentPias.App.Features.Notes.Commands.DeleteNote;
using ManagamentPias.App.Features.Notes.Queries.GetNoteById;
using ManagamentPias.App.Features.Notes.Queries.GetNotes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagamentPias.WebApi.Controllers.v1;

[ApiVersion("1.0")]
public class NotesController : BaseApiController
{
    public NotesController()
    {

    }

    /// <summary>
    /// Gets a list of Notes 
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult> Get([FromQuery] GetNotesQuery filter)
    {
        return Ok(await Mediator!.Send(filter));
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

    //[HttpPut]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> Put(UpdateNoteCommand command)
    //{
    //    var resp = await Mediator!.Send(command);
    //    return NoContent();
    //}

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
