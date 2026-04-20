using BookAuthorApi.Application.Commands.Books;
using BookAuthorApi.Application.DTOs;
using BookAuthorApi.Application.Queries.Books;
using BookAuthorApi.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookAuthorApi.Controllers;

[ApiController]
[Route("books")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetBooks()
    {
        return await ExceptionHelper.HandleExceptionAsync(async () =>
        {
            var books = await _mediator.Send(new GetBooksQuery());
            return Ok(books);
        });
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetBookById(Guid id)
    {
        return await ExceptionHelper.HandleExceptionAsync(async () =>
        {
            var book = await _mediator.Send(new GetBookByIdQuery { Id = id });
            if (book == null) return NotFound(Messages.BookNotFound);
            return Ok(book);
        });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookDto createBookDto)
    {
        return await ExceptionHelper.HandleExceptionAsync(async () =>
        {
            var book = await _mediator.Send(new CreateBookCommand { Book = createBookDto });
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        });
    }

    [HttpPatch("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateBook(Guid id, [FromBody] UpdateBookDto updateBookDto)
    {
        return await ExceptionHelper.HandleExceptionAsync(async () =>
        {
            var book = await _mediator.Send(new UpdateBookCommand { Id = id, Book = updateBookDto });
            if (book == null) return NotFound(Messages.BookNotFound);
            return Ok(book);
        });
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        return await ExceptionHelper.HandleExceptionAsync(async () =>
        {
            var result = await _mediator.Send(new DeleteBookCommand { Id = id });
            if (!result) return NotFound(Messages.BookNotFound);
            return NoContent();
        });
    }

    [HttpGet("validation/{isbn}")]
    public async Task<IActionResult> ValidateIsbn(string isbn)
    {
        return await ExceptionHelper.HandleExceptionAsync(async () =>
        {
            var isValid = await _mediator.Send(new ValidateIsbnQuery { Isbn = isbn });
            return Ok(isValid);
        });
    }

    [HttpPost("masive")]
    public async Task<IActionResult> CreateBooks([FromBody] CreateBooksDto createBooksDto)
    {
        return await ExceptionHelper.HandleExceptionAsync(async () =>
        {
            var books = await _mediator.Send(new CreateBooksCommand { Books = createBooksDto });
            return Ok(books);
        });
    }

    [HttpPost("upload")]
    [Authorize]
    public async Task<IActionResult> UploadBooksCsv(IFormFile file)
    {
        return await ExceptionHelper.HandleExceptionAsync(async () =>
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(Messages.NoValidFileProvided);
            }

            if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(Messages.FileMustBeCsv);
            }

            var result = await _mediator.Send(new UploadBooksCsvCommand { CsvFile = file });

            return Ok(new
            {
                mensaje = Messages.BulkUploadCompleted,
                totalProcesados = result.TotalProcessed,
                creadosExitosamente = result.SuccessfullyCreated,
                fallidos = result.Failed,
                errores = result.Errors,
                librosCreados = result.CreatedBooks
            });
        });
    }
}