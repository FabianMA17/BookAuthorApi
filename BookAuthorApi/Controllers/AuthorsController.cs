using BookAuthorApi.Application.Commands.Authors;
using BookAuthorApi.Application.DTOs;
using BookAuthorApi.Application.Queries.Authors;
using BookAuthorApi.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookAuthorApi.Controllers;

[ApiController]
[Route("authors")]
public class AuthorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAuthors()
    {
        return await ExceptionHelper.HandleExceptionAsync(async () =>
        {
            var authors = await _mediator.Send(new GetAuthorsQuery());
            return Ok(authors);
        });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorDto createAuthorDto)
    {
        return await ExceptionHelper.HandleExceptionAsync(async () =>
        {
            var author = await _mediator.Send(new CreateAuthorCommand { Author = createAuthorDto });
            return Ok(author);
        });
    }

    [HttpPatch("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] UpdateAuthorDto updateAuthorDto)
    {
        return await ExceptionHelper.HandleExceptionAsync(async () =>
        {
            var author = await _mediator.Send(new UpdateAuthorCommand { Id = id, Author = updateAuthorDto });
            if (author == null) return NotFound(Messages.AuthorNotFound);
            return Ok(author);
        });
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteAuthor(Guid id)
    {
        return await ExceptionHelper.HandleExceptionAsync(async () =>
        {
            var result = await _mediator.Send(new DeleteAuthorCommand { Id = id });
            if (!result) return NotFound(Messages.AuthorNotFound);
            return NoContent();
        });
    }
}