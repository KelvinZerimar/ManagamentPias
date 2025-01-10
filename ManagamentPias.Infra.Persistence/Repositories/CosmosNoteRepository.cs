using ManagamentPias.App.Interfaces.Repositories;
using ManagamentPias.Domain.Entities;
using Microsoft.Azure.Cosmos;
using Container = Microsoft.Azure.Cosmos.Container;

namespace ManagamentPias.Infra.Persistence.Repositories;

public class CosmosNoteRepository : INoteRepository
{
    private readonly CosmosClient _cosmosClient;
    private readonly Container _container;

    public CosmosNoteRepository(CosmosClient cosmosClient,
        string databaseName,
        string containerName)
    {
        _cosmosClient = cosmosClient;
        _container = _cosmosClient.GetContainer(databaseName, containerName);
    }

    public async Task<List<Note>> GetAllNotesAsync()
    {
        var query = _container.GetItemQueryIterator<Note>("SELECT * FROM c");
        var results = new List<Note>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response);
        }
        return results;
    }

    public async Task<Note?> GetNoteByIdAsync(Guid id, string partitionKey)
    {
        try
        {
            var response = await _container.ReadItemAsync<Note>(id.ToString(), new PartitionKey(partitionKey));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<Note> CreateNoteAsync(Note note)
    {
        var response = await _container.CreateItemAsync(note, new PartitionKey(note.Category.ToString()));
        return response.Resource;
    }

    public async Task<Note> UpdateNoteAsync(Note note, string partitionKey)
    {
        var response = await _container.UpsertItemAsync(note, new PartitionKey(partitionKey));
        return response.Resource;
    }

    public async Task<bool> DeleteNoteAsync(Guid id, string partitionKey)
    {
        try
        {
            await _container.DeleteItemAsync<Note>(id.ToString(), new PartitionKey(partitionKey));
            return true;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }
}
