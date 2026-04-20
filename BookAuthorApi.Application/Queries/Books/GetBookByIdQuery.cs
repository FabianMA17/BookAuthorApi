using BookAuthorApi.Application.DTOs;
using MediatR;

namespace BookAuthorApi.Application.Queries.Books;

public class GetBookByIdQuery : IRequest<BookDto?>
{
    public Guid Id { get; set; }
}