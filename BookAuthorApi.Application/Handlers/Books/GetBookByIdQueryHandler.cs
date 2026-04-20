using BookAuthorApi.Application.DTOs;
using BookAuthorApi.Application.Queries.Books;
using BookAuthorApi.Domain.Interfaces;
using MediatR;

namespace BookAuthorApi.Application.Handlers.Books;

public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDto?>
{
    private readonly IBookRepository _bookRepository;

    public GetBookByIdQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookDto?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.Id);
        if (book == null)
        {
            return null;
        }

        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Isbn = book.Isbn,
            CoverUrl = book.CoverUrl,
            PublicationYear = book.PublicationYear,
            AuthorId = book.AuthorId,
            AuthorName = book.Author?.Name ?? string.Empty
        };
    }
}