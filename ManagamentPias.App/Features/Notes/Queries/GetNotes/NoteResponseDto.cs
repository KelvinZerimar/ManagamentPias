namespace ManagementPias.App.Features.Notes.Queries.GetNotes;

public record NoteResponseDto
{
    public string Id { get; set; } = null!;
    public string CategoryEnum { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string RichTextContent { get; set; } = null!;
    public DateTime CreateDate { get; init; } = DateTime.Now;
}
