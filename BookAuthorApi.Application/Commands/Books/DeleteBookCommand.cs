using MediatR;

namespace BookAuthorApi.Application.Commands.Books;

public class DeleteBookCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}