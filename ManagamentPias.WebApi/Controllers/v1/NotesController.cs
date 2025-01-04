using Asp.Versioning;
using ManagamentPias.App.Features.Notes.Commands.CreateNote;
using ManagamentPias.App.Features.Notes.Commands.DeleteNote;
using ManagamentPias.App.Features.Notes.Queries.GetNoteById;
using ManagamentPias.App.Features.Notes.Queries.GetNotes;
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
    public async Task<ActionResult> Get([FromQuery] GetNotesQuery filter)
    {
        return Ok(await Mediator!.Send(filter));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Get(Guid id, [FromQuery] string partitionKey)
    {
        return Ok(await Mediator!.Send(new GetNoteByIdQuery { Id = id, PartitionKey = partitionKey }));
    }

    [HttpPost]
    // [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(CreateNoteCommand command)
    {
        var resp = await Mediator!.Send(command);
        return CreatedAtAction(nameof(Post), resp);
    }

    //[HttpPut]
    //// [Authorize]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> Put(UpdateNoteCommand command)
    //{
    //    var resp = await Mediator!.Send(command);
    //    return NoContent();
    //}

    [HttpDelete("{id}")]
    // [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id, [FromQuery] string partitionKey)
    {
        var command = new DeleteNoteCommand { Id = id, PartitionKey = partitionKey };
        var resp = await Mediator!.Send(command);
        return NoContent();
    }
}
