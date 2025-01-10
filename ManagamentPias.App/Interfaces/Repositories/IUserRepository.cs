using ManagamentPias.Domain.Entities;

namespace ManagamentPias.App.Interfaces.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User?> GetUserByIdAsync(Guid id, string partitionKey);
    Task<User> RegisterUserAsync(User user);

    Task<User?> GetUserByEmailAsync(string email);
}
