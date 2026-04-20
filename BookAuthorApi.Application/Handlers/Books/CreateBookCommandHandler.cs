using BookAuthorApi.Application.Commands.Books;
using BookAuthorApi.Application.DTOs;
using BookAuthorApi.Domain.Interfaces;
using MediatR;

namespace BookAuthorApi.Application.Handlers.Books;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookDto>
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookCoverService _bookCoverService;

    public CreateBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository, IBookCoverService bookCoverService)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _bookCoverService = bookCoverService;
    }

    public async Task<BookDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        // Validate that the author exists
        var author = await _authorRepository.GetByIdAsync(request.Book.AuthorId);
        if (author == null)
        {
            throw new ArgumentException("El autor especificado no existe");
        }

        // Get cover URL from Open Library API if not provided
        var coverUrl = request.Book.CoverUrl ?? string.Empty;
        if (string.IsNullOrEmpty(coverUrl))
        {
            coverUrl = await _bookCoverService.GetBookCoverUrlAsync(request.Book.Isbn) ?? string.Empty;
        }

        var book = new BookAuthorApi.Domain.Entities.Book
        {
            Title = request.Book.Title,
            Isbn = request.Book.Isbn,
            PublicationYear = request.Book.PublicationYear,
            CoverUrl = coverUrl,
            AuthorId = request.Book.AuthorId
        };

        await _bookRepository.AddAsync(book);

        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Isbn = book.Isbn,
            CoverUrl = book.CoverUrl,
            PublicationYear = book.PublicationYear,
            AuthorId = book.AuthorId,
            AuthorName = author.Name
        };
    }
}