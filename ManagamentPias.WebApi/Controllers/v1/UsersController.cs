using Asp.Versioning;
using ManagamentPias.App.Features.Users.Commands.RegisterUser;
using ManagamentPias.App.Features.Users.Queries.Login;
using Microsoft.AspNetCore.Mvc;

namespace ManagamentPias.WebApi.Controllers.v1;

[ApiVersion("1.0")]
public class UsersController : BaseApiController
{
    public UsersController()
    {
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginUserQuery query)
    {
        return Ok(await Mediator!.Send(new LoginUserQuery { Email = query.Email, Password = query.Password }));
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        var resp = await Mediator!.Send(command);
        return CreatedAtAction(nameof(Register), resp);
    }
    //[HttpPut]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> Put(UpdateUserCommand command)
    //{
    //    var resp = await Mediator!.Send(command);
    //    return NoContent();
    //}
    //[HttpDelete("{id}")]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> Delete(Guid id, [FromQuery] string partitionKey)
    //{
    //    await Mediator!.Send(new DeleteUserCommand { Id = id, PartitionKey = partitionKey });
    //    return NoContent();
    //}
}
