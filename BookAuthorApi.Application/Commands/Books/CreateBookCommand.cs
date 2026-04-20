using BookAuthorApi.Application.DTOs;
using MediatR;

namespace BookAuthorApi.Application.Commands.Books;

public class CreateBookCommand : IRequest<BookDto>
{
    public CreateBookDto Book { get; set; } = new();
}