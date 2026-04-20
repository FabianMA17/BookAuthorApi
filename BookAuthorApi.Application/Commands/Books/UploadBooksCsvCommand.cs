using BookAuthorApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookAuthorApi.Application.Commands.Books;

public class UploadBooksCsvCommand : IRequest<UploadBooksResult>
{
    public IFormFile CsvFile { get; set; } = null!;
}

public class UploadBooksResult
{
    public int TotalProcessed { get; set; }
    public int SuccessfullyCreated { get; set; }
    public int Failed { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<BookDto> CreatedBooks { get; set; } = new();
}