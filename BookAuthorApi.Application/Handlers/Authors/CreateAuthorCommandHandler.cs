using BookAuthorApi.Application.Commands.Authors;
using BookAuthorApi.Application.DTOs;
using BookAuthorApi.Domain.Interfaces;
using MediatR;

namespace BookAuthorApi.Application.Handlers.Authors;

public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, AuthorDto>
{
    private readonly IAuthorRepository _authorRepository;

    public CreateAuthorCommandHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<AuthorDto> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = new Domain.Entities.Author
        {
            Name = request.Author.Name
        };

        await _authorRepository.AddAsync(author);

        return new AuthorDto
        {
            Id = author.Id,
            Name = author.Name
        };
    }
}