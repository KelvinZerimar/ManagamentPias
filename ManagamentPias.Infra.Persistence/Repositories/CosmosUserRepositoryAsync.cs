using ManagementPias.App.Interfaces.Repositories;
using Microsoft.Azure.Cosmos;
using User = ManagementPias.Domain.Entities.User;

namespace ManagementPias.Infra.Persistence.Repositories;

public class CosmosUserRepositoryAsync : ICosmosUserRepository
{
    private readonly CosmosClient _cosmosClient;
    private readonly Container _container;

    public CosmosUserRepositoryAsync(CosmosClient cosmosClient,
        string databaseName,
        string containerName)
    {
        _cosmosClient = cosmosClient;
        _container = _cosmosClient.GetContainer(databaseName, containerName);
    }

    public async Task<User?> GetUserByIdAsync(Guid id, string partitionKey)
    {
        try
        {
            var response = await _container.ReadItemAsync<User>(id.ToString(), new PartitionKey(partitionKey));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        var query = _container.GetItemQueryIterator<User>();
        List<User> results = new();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response);
        }
        return results;
    }

    public async Task<User> RegisterUserAsync(User user)
    {
        var response = await _container.CreateItemAsync(user, new PartitionKey(user.Email));
        return response.Resource;
    }

    public async Task<User> UpdateUserAsync(User user, string partitionKey)
    {
        var response = await _container.UpsertItemAsync(user, new PartitionKey(partitionKey));
        return response.Resource;
    }

    public async Task<bool> DeleteUserAsync(Guid id, string partitionKey)
    {
        try
        {
            await _container.DeleteItemAsync<User>(id.ToString(), new PartitionKey(partitionKey));
            return true;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var query = _container.GetItemQueryIterator<User>(new QueryDefinition($"SELECT * FROM c WHERE c.email = '{email}'"));
        var results = new List<User>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response);
        }
        return results.FirstOrDefault();
    }
}
