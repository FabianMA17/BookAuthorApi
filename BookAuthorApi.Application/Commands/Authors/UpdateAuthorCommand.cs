using BookAuthorApi.Application.DTOs;
using MediatR;

namespace BookAuthorApi.Application.Commands.Authors;

public class UpdateAuthorCommand : IRequest<AuthorDto?>
{
    public Guid Id { get; set; }
    public UpdateAuthorDto Author { get; set; } = new();
}