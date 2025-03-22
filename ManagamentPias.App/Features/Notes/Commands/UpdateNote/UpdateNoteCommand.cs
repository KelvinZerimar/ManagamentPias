using AutoMapper;
using ManagementPias.App.Common.Services;
using ManagementPias.App.Features.Notes.Commands.CreateNote;
using ManagementPias.App.Interfaces.Repositories;
using ManagementPias.App.Wrappers;
using ManagementPias.Domain.Entities;
using MediatR;

namespace ManagementPias.App.Features.Notes.Commands.UpdateNote;

public class UpdateNoteCommand : NoteRequestDto, IRequest<Response<NoteRequestDto>>
{
    public Guid Id { get; set; }
}

public class UpdateNoteCommandHandler(INoteRepository _repository,
    IMapper _mapper,
    IEncryptionService _encryptionService) : IRequestHandler<UpdateNoteCommand, Response<NoteRequestDto>>
{
    public async Task<Response<NoteRequestDto>> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
    {
        var note = _mapper.Map<NoteRequestDto>(request);
        var updatedNote = Note.UpdateNoteAsync(
            id: request.Id,
            category: note.Category.ToString(),
            title: note.Title,
            richTextContent: _encryptionService.Encrypt(note.RichTextContent),
            createDate: note.CreateDate,
            modifyDate: note.ModifyDate,
            isActive: note.IsActive
        );
        await _repository.UpdateNoteAsync(updatedNote, request.Category.ToString());
        return new Response<NoteRequestDto>(note);
    }
}
