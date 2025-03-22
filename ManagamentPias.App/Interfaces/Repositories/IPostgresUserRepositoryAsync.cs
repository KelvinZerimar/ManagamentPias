using ManagementPias.Domain.Entities;

namespace ManagementPias.App.Interfaces.Repositories;

public interface IPostgresUserRepositoryAsync : IGenericRepositoryAsync<User>
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByUsernameAsync(string username);
}
