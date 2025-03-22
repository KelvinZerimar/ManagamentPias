using ManagementPias.App.Common.Services;
using ManagementPias.App.Helpers;
using ManagementPias.App.Interfaces.Repositories;
using ManagementPias.App.Wrappers;
using MediatR;

namespace ManagementPias.App.Features.Users.Queries.Login;

public class PostgresLoginUserQuery() : IRequest<Response<SignInData>>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

sealed class PostgresLoginUserQueryHandler(IPostgresUserRepositoryAsync _repository, ITokenService _tokenService)
    : IRequestHandler<PostgresLoginUserQuery, Response<SignInData>>
{
    public async Task<Response<SignInData>> Handle(PostgresLoginUserQuery request, CancellationToken cancellationToken)
    {
        var response = await _repository.GetUserByEmailAsync(request.Email);
        if (response is null) throw new KeyNotFoundException($"User {request.Email} not found");
        if (response == null || !PasswordHelper.VerifyPassword(request.Password, response.PasswordHash))
        {
            throw new UnauthorizedAccessException("Correo o contraseña incorrectos.");
        }
        if (!response.IsActive)
        {
            throw new UnauthorizedAccessException("La cuenta está desactivada.");
        }
        var token = _tokenService.CreateAuthenticationToken(response.Id, response.Name);
        return new Response<SignInData>(
        new SignInData()
        {
            Result = MySignInResult.Success,
            Username = response.Name,
            Email = response.Email,
            Token = token
        });
    }
}
