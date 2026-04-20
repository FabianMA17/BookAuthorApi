namespace BookAuthorApi.Domain.Services;

public interface IIsbnValidator
{
    bool IsValidIsbn(string isbn);
}