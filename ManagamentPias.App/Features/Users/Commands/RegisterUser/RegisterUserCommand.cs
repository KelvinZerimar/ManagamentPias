using AutoMapper;
using ManagamentPias.App.Helpers;
using ManagamentPias.App.Interfaces.Repositories;
using ManagamentPias.App.Wrappers;
using ManagamentPias.Domain.Entities;
using MediatR;

namespace ManagamentPias.App.Features.Users.Commands.RegisterUser;

public class RegisterUserDto { public string Name { get; set; } = null!; public string Email { get; set; } = null!; public string Password { get; set; } = null!; };
public class RegisterUserCommand : RegisterUserDto, IRequest<Response<RegisterUserDto>>
{
}

public class CreateUserCommandHandler(IUserRepository _repository,
    IMapper _mapper) : IRequestHandler<RegisterUserCommand, Response<RegisterUserDto>>
{
    public async Task<Response<RegisterUserDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<RegisterUserDto>(request);
        // Verificar si el correo ya está registrado
        var existingUser = await _repository.GetUsersAsync();
        if (existingUser.Any(u => u.Email == user.Email))
        {
            throw new Exception("El correo ya está registrado.");
        }

        await _repository.RegisterUserAsync(User.Create(
            name: user.Name,
            email: user.Email,
            passwordHash: PasswordHelper.HashPassword(user.Password), // Hash de contraseña
            role: "User", // Rol predeterminado
            isActive: true,
            isVerified: false
        ));

        return new Response<RegisterUserDto>(user);
    }
}