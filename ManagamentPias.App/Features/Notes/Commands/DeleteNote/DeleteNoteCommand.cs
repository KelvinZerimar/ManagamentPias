using ManagamentPias.App.Interfaces.Repositories;
using ManagamentPias.App.Wrappers;
using MediatR;

namespace ManagamentPias.App.Features.Notes.Commands.DeleteNote;

public class DeleteNoteCommand : IRequest<Response<bool>>
{
    public Guid Id { get; set; }
    public string PartitionKey { get; set; } = null!;
    public class DeleteNoteCommandHandler(INoteRepository _repository) : IRequestHandler<DeleteNoteCommand, Response<bool>>
    {
        public async Task<Response<bool>> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            var response = await _repository.DeleteNoteAsync(request.Id, request.PartitionKey);

            return new Response<bool>(response);
        }
    }
}
