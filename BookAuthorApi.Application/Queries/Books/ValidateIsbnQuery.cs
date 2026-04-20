using MediatR;

namespace BookAuthorApi.Application.Queries.Books;

public class ValidateIsbnQuery : IRequest<bool>
{
    public string Isbn { get; set; } = string.Empty;
}