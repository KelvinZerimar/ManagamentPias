using ManagementPias.App.Interfaces.Repositories;
using ManagementPias.Domain.Entities;
using ManagementPias.Infra.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ManagementPias.Infra.Persistence.Repositories;

public class PostgresUserRepositoryAsync : GenericRepositoryAsync<User>, IPostgresUserRepositoryAsync
{
    private readonly DbSet<User> _repository;

    public PostgresUserRepositoryAsync(ApplicationDbContext dbContext,
            ILogger<AssetRepositoryAsync> logger) : base(dbContext)
    {
        _repository = dbContext.Set<User>();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _repository.FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _repository.FirstOrDefaultAsync(user => user.Name == username);
    }
}
