using ManagementPias.App.Common.Services;
using ManagementPias.App.Interfaces.Repositories;
using ManagementPias.App.Wrappers;
using MediatR;

namespace ManagementPias.App.Features.Notes.Queries.GetNotes;

public class GetNotesQuery : IRequest<Response<IEnumerable<NoteResponseDto>>>
{
    public class GetNotesQueryHandler(INoteRepository noteRepository,
        IEncryptionService _encryptionService) : IRequestHandler<GetNotesQuery, Response<IEnumerable<NoteResponseDto>>>
    {
        public async Task<Response<IEnumerable<NoteResponseDto>>> Handle(GetNotesQuery request, CancellationToken cancellationToken)
        {
            var response = await noteRepository.GetAllNotesAsync();

            return new Response<IEnumerable<NoteResponseDto>>(response.Select(resp => new NoteResponseDto
            {
                Id = resp.Id.ToString(),
                CategoryEnum = resp.Category,
                Title = resp.Title,
                RichTextContent = _encryptionService.Decrypt(resp.RichTextContent),
                CreateDate = resp.CreateDate
            }).OrderByDescending(n => n.CreateDate));
        }
    }
}
