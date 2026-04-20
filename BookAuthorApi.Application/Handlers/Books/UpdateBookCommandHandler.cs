using BookAuthorApi.Application.Commands.Books;
using BookAuthorApi.Application.DTOs;
using BookAuthorApi.Domain.Interfaces;
using MediatR;

namespace BookAuthorApi.Application.Handlers.Books;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookDto?>
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;

    public UpdateBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
    }

    public async Task<BookDto?> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.Id);
        if (book == null)
        {
            return null;
        }

        // Update fields if provided
        if (!string.IsNullOrEmpty(request.Book.Title))
            book.Title = request.Book.Title;

        if (!string.IsNullOrEmpty(request.Book.Isbn))
            book.Isbn = request.Book.Isbn;

        if (!string.IsNullOrEmpty(request.Book.CoverUrl))
            book.CoverUrl = request.Book.CoverUrl;

        if (request.Book.PublicationYear.HasValue)
            book.PublicationYear = request.Book.PublicationYear.Value;

        if (request.Book.AuthorId.HasValue)
        {
            // Validate that the author exists
            var author = await _authorRepository.GetByIdAsync(request.Book.AuthorId.Value);
            if (author == null)
            {
                throw new ArgumentException("El autor especificado no existe");
            }
            book.AuthorId = request.Book.AuthorId.Value;
        }

        await _bookRepository.UpdateAsync(book);

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