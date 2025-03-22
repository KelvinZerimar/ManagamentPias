using ManagementPias.Domain.Entities;

namespace ManagementPias.App.Interfaces.Repositories;

public interface ICosmosUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User?> GetUserByIdAsync(Guid id, string partitionKey);
    Task<User> RegisterUserAsync(User user);

    Task<User?> GetUserByEmailAsync(string email);
}
