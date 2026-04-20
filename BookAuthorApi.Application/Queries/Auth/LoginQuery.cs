using BookAuthorApi.Application.DTOs;
using MediatR;

namespace BookAuthorApi.Application.Queries.Auth;

public class LoginQuery : IRequest<LoginResponse>
{
    public LoginRequest Login { get; set; } = new();
}