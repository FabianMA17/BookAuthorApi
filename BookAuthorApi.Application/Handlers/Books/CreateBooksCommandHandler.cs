using BookAuthorApi.Application.Commands.Books;
using BookAuthorApi.Application.DTOs;
using BookAuthorApi.Domain.Interfaces;
using MediatR;

namespace BookAuthorApi.Application.Handlers.Books;

public class CreateBooksCommandHandler : IRequestHandler<CreateBooksCommand, IEnumerable<BookDto>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookCoverService _bookCoverService;

    public CreateBooksCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository, IBookCoverService bookCoverService)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _bookCoverService = bookCoverService;
    }

    public async Task<IEnumerable<BookDto>> Handle(CreateBooksCommand request, CancellationToken cancellationToken)
    {
        var createdBooks = new List<BookDto>();

        foreach (var createBookDto in request.Books.Books)
        {
            // Validate that the author exists
            var author = await _authorRepository.GetByIdAsync(createBookDto.AuthorId);
            if (author == null)
            {
                throw new ArgumentException($"El autor especificado no existe para el libro: {createBookDto.Title}");
            }

            // Get cover URL from Open Library API if not provided
            var coverUrl = createBookDto.CoverUrl ?? string.Empty;
            if (string.IsNullOrEmpty(coverUrl))
            {
                coverUrl = await _bookCoverService.GetBookCoverUrlAsync(createBookDto.Isbn) ?? string.Empty;
            }

            var book = new BookAuthorApi.Domain.Entities.Book
            {
                Title = createBookDto.Title,
                Isbn = createBookDto.Isbn,
                PublicationYear = createBookDto.PublicationYear,
                CoverUrl = coverUrl,
                AuthorId = createBookDto.AuthorId
            };

            await _bookRepository.AddAsync(book);

            createdBooks.Add(new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Isbn = book.Isbn,
                CoverUrl = book.CoverUrl,
                PublicationYear = book.PublicationYear,
                AuthorId = book.AuthorId,
                AuthorName = author.Name
            });
        }

        return createdBooks;
    }
}