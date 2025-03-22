using AutoMapper;
using ManagementPias.App.Helpers;
using ManagementPias.App.Interfaces.Repositories;
using ManagementPias.App.Wrappers;
using ManagementPias.Domain.Entities;
using MediatR;

namespace ManagementPias.App.Features.Users.Commands.RegisterUser;

public class PostgresRegisterUserCommand : RegisterUserDto, IRequest<Response<RegisterUserDto>>
{
}

// ...

sealed class PostgresRegisterUserCommandHandler(IPostgresUserRepositoryAsync _repository,
    IMapper _mapper)
    : IRequestHandler<PostgresRegisterUserCommand, Response<RegisterUserDto>>
{
    public async Task<Response<RegisterUserDto>> Handle(PostgresRegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<RegisterUserDto>(request);
        // Verificar si el correo ya está registrado
        var existingUser = await _repository.GetUserByEmailAsync(request.Email);
        if (existingUser != null) // Cambiar la verificación
        {
            throw new Exception("El correo ya está registrado.");
        }

        await _repository.AddAsync(User.Create(
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
