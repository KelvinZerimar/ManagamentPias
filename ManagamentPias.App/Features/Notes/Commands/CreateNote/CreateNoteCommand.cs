using AutoMapper;
using ManagamentPias.App.Common.Services;
using ManagamentPias.App.Interfaces.Repositories;
using ManagamentPias.App.Wrappers;
using ManagamentPias.Domain.Entities;
using MediatR;

namespace ManagamentPias.App.Features.Notes.Commands.CreateNote;

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