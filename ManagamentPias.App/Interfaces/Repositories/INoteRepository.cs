using ManagamentPias.Domain.Entities;

namespace ManagamentPias.App.Interfaces.Repositories;

public interface INoteRepository
{
    Task<List<Note>> GetAllNotesAsync();
    Task<Note?> GetNoteByIdAsync(Guid id, string partitionKey);
    Task<Note> CreateNoteAsync(Note note);
    Task<Note> UpdateNoteAsync(Note note, string partitionKey);
    Task<bool> DeleteNoteAsync(Guid id, string partitionKey);
}
