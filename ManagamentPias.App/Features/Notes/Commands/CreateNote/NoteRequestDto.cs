using ManagamentPias.Domain.Enums;

namespace ManagamentPias.App.Features.Notes.Commands.CreateNote
{
    public class NoteRequestDto
    {
        public CategoryNote Category { get; set; }
        public string Title { get; set; } = null!;
        public string RichTextContent { get; set; } = null!;
        public DateTime CreateDate { get; init; } = DateTime.Now;
        public DateTime? ModifyDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
