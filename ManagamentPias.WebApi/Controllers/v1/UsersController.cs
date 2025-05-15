using Asp.Versioning;
using ManagementPias.App.Features.Users.Commands.RegisterUser;
using ManagementPias.App.Features.Users.Queries.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace ManagementPias.WebApi.Controllers.v1;

[ApiVersion("1.0")]
public class UsersController(ILogger<AssetsController> logger, HybridCache hybridCache) : BaseApiController
{
    #region PostGres
    [HttpPost("login")]
    public async Task<ActionResult> LoginPostGres(PostgresLoginUserQuery query, CancellationToken ct)
    {
        logger.LogInformation(message: "Request received to LoginPostGres.");
        var result = await hybridCache.GetOrCreateAsync("login-postgres", async ct =>
        {
            return await Mediator!.Send(query, ct);
        },
        tags: ["LoginPostgres"],
        cancellationToken: ct);
        logger.LogInformation(message: "LoginPostGres retrieved successfully.");
        return Ok(result);
        //return Ok(await Mediator!.Send(new PostgresLoginUserQuery { Email = query.Email, Password = query.Password }));
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterPostGres(PostgresRegisterUserCommand command)
    {
        var resp = await Mediator!.Send(command);
        return CreatedAtAction(nameof(RegisterPostGres), resp);
    }
    #endregion

    #region Cosmos
    [HttpPost("login-cosmos")]
    public async Task<ActionResult> LoginCosmos(CosmosLoginUserQuery query)
    {
        return Ok(await Mediator!.Send(new CosmosLoginUserQuery { Email = query.Email, Password = query.Password }));
    }

    [HttpPost("register-cosmos")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterCosmos(CosmosRegisterUserCommand command)
    {
        var resp = await Mediator!.Send(command);
        return CreatedAtAction(nameof(RegisterCosmos), resp);
    }
    #endregion

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
