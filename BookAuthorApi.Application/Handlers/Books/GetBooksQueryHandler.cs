using BookAuthorApi.Application.DTOs;
using BookAuthorApi.Application.Queries.Books;
using BookAuthorApi.Domain.Interfaces;
using MediatR;

namespace BookAuthorApi.Application.Handlers.Books;

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, IEnumerable<BookDto>>
{
    private readonly IBookRepository _bookRepository;

    public GetBooksQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetAllAsync();
        return books.Select(b => new BookDto
        {
            Id = b.Id,
            Title = b.Title,
            Isbn = b.Isbn,
            CoverUrl = b.CoverUrl,
            PublicationYear = b.PublicationYear,
            AuthorId = b.AuthorId,
            AuthorName = b.Author?.Name ?? string.Empty
        });
    }
}