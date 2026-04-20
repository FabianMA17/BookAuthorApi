using BookAuthorApi.Application.Commands.Books;
using BookAuthorApi.Application.DTOs;
using BookAuthorApi.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace BookAuthorApi.Application.Handlers.Books;

public class UploadBooksCsvCommandHandler : IRequestHandler<UploadBooksCsvCommand, UploadBooksResult>
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookCoverService _bookCoverService;

    public UploadBooksCsvCommandHandler(
        IBookRepository bookRepository,
        IAuthorRepository authorRepository,
        IBookCoverService bookCoverService)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _bookCoverService = bookCoverService;
    }

    public async Task<UploadBooksResult> Handle(UploadBooksCsvCommand request, CancellationToken cancellationToken)
    {
        var result = new UploadBooksResult();

        try
        {
            // Parse CSV content
            var csvLines = await ParseCsvFileAsync(request.CsvFile);
            result.TotalProcessed = csvLines.Count;

            foreach (var line in csvLines)
            {
                try
                {
                    var bookData = ParseCsvLine(line);
                    if (bookData == null)
                    {
                        result.Failed++;
                        result.Errors.Add($"Línea inválida: {line}");
                        continue;
                    }

                    // Ensure author exists or create it
                    var author = await GetOrCreateAuthorAsync(bookData.AuthorName);
                    if (author == null)
                    {
                        result.Failed++;
                        result.Errors.Add($"Error al crear/obtener autor: {bookData.AuthorName}");
                        continue;
                    }

                    // Get cover URL
                    var coverUrl = await _bookCoverService.GetBookCoverUrlAsync(bookData.Isbn) ?? string.Empty;

                    // Create book
                    var book = new Domain.Entities.Book
                    {
                        Title = bookData.Title,
                        Isbn = bookData.Isbn,
                        PublicationYear = bookData.PublicationYear,
                        CoverUrl = coverUrl,
                        AuthorId = author.Id
                    };

                    await _bookRepository.AddAsync(book);

                    var bookDto = new BookDto
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Isbn = book.Isbn,
                        CoverUrl = book.CoverUrl,
                        PublicationYear = book.PublicationYear,
                        AuthorId = book.AuthorId,
                        AuthorName = author.Name
                    };

                    result.CreatedBooks.Add(bookDto);
                    result.SuccessfullyCreated++;
                }
                catch (Exception ex)
                {
                    result.Failed++;
                    result.Errors.Add($"Error procesando línea '{line}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            result.Errors.Add($"Error general procesando archivo CSV: {ex.Message}");
        }

        return result;
    }

    private async Task<List<string>> ParseCsvFileAsync(IFormFile file)
    {
        var lines = new List<string>();

        using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
        {
            string? line;
            bool isFirstLine = true;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                // Skip header line
                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(line))
                {
                    lines.Add(line);
                }
            }
        }

        return lines;
    }

    private CsvBookData? ParseCsvLine(string line)
    {
        // Expected format: isbn,title,publicationYear,authorName
        var parts = line.Split(',');

        if (parts.Length != 4)
        {
            return null;
        }

        if (!int.TryParse(parts[2].Trim(), out var publicationYear))
        {
            return null;
        }

        return new CsvBookData
        {
            Isbn = parts[0].Trim(),
            Title = parts[1].Trim(),
            PublicationYear = publicationYear,
            AuthorName = parts[3].Trim()
        };
    }

    private async Task<Domain.Entities.Author?> GetOrCreateAuthorAsync(string authorName)
    {
        // Try to find existing author (case-insensitive)
        var existingAuthors = await _authorRepository.GetAllAsync();
        var existingAuthor = existingAuthors.FirstOrDefault(a =>
            a.Name.Equals(authorName, StringComparison.OrdinalIgnoreCase));

        if (existingAuthor != null)
        {
            return existingAuthor;
        }

        // Create new author
        var newAuthor = new Domain.Entities.Author
        {
            Name = authorName
        };

        await _authorRepository.AddAsync(newAuthor);
        return newAuthor;
    }

    private class CsvBookData
    {
        public string Isbn { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public string AuthorName { get; set; } = string.Empty;
    }
}