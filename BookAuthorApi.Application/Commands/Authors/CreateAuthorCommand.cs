using BookAuthorApi.Application.DTOs;
using MediatR;

namespace BookAuthorApi.Application.Commands.Authors;

public class CreateAuthorCommand : IRequest<AuthorDto>
{
    public CreateAuthorDto Author { get; set; } = new();
}