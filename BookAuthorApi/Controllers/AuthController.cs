using Microsoft.AspNetCore.Mvc;
using MediatR;
using BookAuthorApi.Application.Queries.Auth;
using BookAuthorApi.Application.DTOs;

namespace BookAuthorApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login([FromQuery] LoginRequest loginRequest)
    {
        try
        {
            var response = await _mediator.Send(new LoginQuery { Login = loginRequest });
            return Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = Messages.InvalidUsernameOrPassword });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = Messages.LoginError, details = ex.Message });
        }
    }
}