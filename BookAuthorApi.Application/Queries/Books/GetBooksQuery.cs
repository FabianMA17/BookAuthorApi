using BookAuthorApi.Application.DTOs;
using MediatR;

namespace BookAuthorApi.Application.Queries.Books;

public class GetBooksQuery : IRequest<IEnumerable<BookDto>>
{
}