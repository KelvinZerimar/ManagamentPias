using AutoMapper;
using ManagementPias.App.Common.Services;
using ManagementPias.App.Interfaces.Repositories;
using ManagementPias.App.Wrappers;
using ManagementPias.Domain.Entities;
using MediatR;

namespace ManagementPias.App.Features.Notes.Commands.CreateNote;

public class CreateNoteCommand : NoteRequestDto, IRequest<Response<NoteRequestDto>>
{
}

public class CreateNoteCommandHandler(INoteRepository _repository,
    IMapper _mapper,
    IEncryptionService _encryptionService) : IRequestHandler<CreateNoteCommand, Response<NoteRequestDto>>
{
    public async Task<Response<NoteRequestDto>> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        var note = _mapper.Map<NoteRequestDto>(request);
        await _repository.CreateNoteAsync(Note.Create(
            category: note.Category.ToString(),
            title: note.Title,
            richTextContent: _encryptionService.Encrypt(note.RichTextContent),
            createDate: note.CreateDate,
            modifyDate: note.ModifyDate,
            isActive: note.IsActive)
            );
        return new Response<NoteRequestDto>(note);
    }
}