using BookAuthorApi.Application.DTOs;
using BookAuthorApi.Application.Queries.Authors;
using BookAuthorApi.Domain.Interfaces;
using MediatR;

namespace BookAuthorApi.Application.Handlers.Authors;

public class GetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, IEnumerable<AuthorDto>>
{
    private readonly IAuthorRepository _authorRepository;

    public GetAuthorsQueryHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<IEnumerable<AuthorDto>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authors = await _authorRepository.GetAllAsync();
        return authors.Select(a => new AuthorDto
        {
            Id = a.Id,
            Name = a.Name
        });
    }
}