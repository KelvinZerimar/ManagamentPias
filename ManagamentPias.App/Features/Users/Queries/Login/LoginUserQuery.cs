using ManagamentPias.App.Common.Services;
using ManagamentPias.App.Helpers;
using ManagamentPias.App.Interfaces.Repositories;
using ManagamentPias.App.Wrappers;
using MediatR;

namespace ManagamentPias.App.Features.Users.Queries.Login;

public class LoginUserQuery() : IRequest<Response<SignInData?>>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public class LoginUserQueryHandler(IUserRepository userRepository, ITokenService _tokenService) : IRequestHandler<LoginUserQuery, Response<SignInData?>>
    {
        public async Task<Response<SignInData?>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var response = await userRepository.GetUserByEmailAsync(request.Email);
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

            return new Response<SignInData?>(
            new SignInData()
            {
                Result = MySignInResult.Success,
                Username = response.Name,
                Email = response.Email,
                Token = token
            });
        }
    }
}
