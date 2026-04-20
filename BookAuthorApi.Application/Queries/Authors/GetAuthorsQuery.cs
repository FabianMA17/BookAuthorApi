using BookAuthorApi.Application.DTOs;
using MediatR;

namespace BookAuthorApi.Application.Queries.Authors;

public class GetAuthorsQuery : IRequest<IEnumerable<AuthorDto>>
{
}