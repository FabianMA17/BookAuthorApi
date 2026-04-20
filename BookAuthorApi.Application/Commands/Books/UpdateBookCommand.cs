using BookAuthorApi.Application.DTOs;
using MediatR;

namespace BookAuthorApi.Application.Commands.Books;

public class UpdateBookCommand : IRequest<BookDto?>
{
    public Guid Id { get; set; }
    public UpdateBookDto Book { get; set; } = new();
}