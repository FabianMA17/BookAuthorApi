using BookAuthorApi.Application.Commands.Authors;
using BookAuthorApi.Application.DTOs;
using BookAuthorApi.Domain.Interfaces;
using MediatR;

namespace BookAuthorApi.Application.Handlers.Authors;

public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, AuthorDto?>
{
    private readonly IAuthorRepository _authorRepository;

    public UpdateAuthorCommandHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<AuthorDto?> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await _authorRepository.GetByIdAsync(request.Id);
        if (author == null) return null;

        if (request.Author.Name != null) author.Name = request.Author.Name;

        await _authorRepository.UpdateAsync(author);

        return new AuthorDto
        {
            Id = author.Id,
            Name = author.Name
        };
    }
}