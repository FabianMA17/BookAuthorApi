using BookAuthorApi.Application.Commands.Authors;
using BookAuthorApi.Domain.Interfaces;
using MediatR;

namespace BookAuthorApi.Application.Handlers.Authors;

public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, bool>
{
    private readonly IAuthorRepository _authorRepository;

    public DeleteAuthorCommandHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<bool> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        var exists = await _authorRepository.ExistsAsync(request.Id);
        if (!exists) return false;

        await _authorRepository.DeleteAsync(request.Id);
        return true;
    }
}