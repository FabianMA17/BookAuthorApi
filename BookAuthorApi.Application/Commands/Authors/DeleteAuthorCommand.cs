using MediatR;

namespace BookAuthorApi.Application.Commands.Authors;

public class DeleteAuthorCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}