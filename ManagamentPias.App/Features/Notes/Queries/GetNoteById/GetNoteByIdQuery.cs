﻿using ManagamentPias.App.Common.Services;
using ManagamentPias.App.Features.Notes.Queries.GetNotes;
using ManagamentPias.App.Interfaces.Repositories;
using ManagamentPias.App.Wrappers;
using MediatR;

namespace ManagamentPias.App.Features.Notes.Queries.GetNoteById;

public class GetNoteByIdQuery : IRequest<Response<NoteResponseDto>>
{
    public Guid Id { get; set; }
    public string PartitionKey { get; set; } = null!;
    public class GetNoteByIdQueryHandler(INoteRepository noteRepository,
        IEncryptionService _encryptionService) : IRequestHandler<GetNoteByIdQuery, Response<NoteResponseDto>>
    {
        public async Task<Response<NoteResponseDto>> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await noteRepository.GetNoteByIdAsync(request.Id, request.PartitionKey);
            if (response is null) throw new KeyNotFoundException($"Note {request.Id} not found");

            return new Response<NoteResponseDto>(new NoteResponseDto
            {
                Id = response.Id.ToString(),
                CategoryEnum = response.Category,
                Title = response.Title,
                RichTextContent = _encryptionService.Decrypt(response.RichTextContent),
                CreateDate = response.CreateDate
            });
        }
    }
}