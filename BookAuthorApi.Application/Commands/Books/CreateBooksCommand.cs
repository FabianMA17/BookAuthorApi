using BookAuthorApi.Application.DTOs;
using MediatR;

namespace BookAuthorApi.Application.Commands.Books;

public class CreateBooksCommand : IRequest<IEnumerable<BookDto>>
{
    public CreateBooksDto Books { get; set; } = new();
}