using BookAuthorApi.Application.Queries.Books;
using BookAuthorApi.Domain.Services;
using MediatR;

namespace BookAuthorApi.Application.Handlers.Books;

public class ValidateIsbnQueryHandler : IRequestHandler<ValidateIsbnQuery, bool>
{
    private readonly IIsbnValidator _isbnValidator;

    public ValidateIsbnQueryHandler(IIsbnValidator isbnValidator)
    {
        _isbnValidator = isbnValidator;
    }

    public async Task<bool> Handle(ValidateIsbnQuery request, CancellationToken cancellationToken)
    {
        return _isbnValidator.IsValidIsbn(request.Isbn);
    }
}