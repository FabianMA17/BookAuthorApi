namespace BookAuthorApi.Domain.Interfaces;

public interface IBookCoverService
{
    Task<string?> GetBookCoverUrlAsync(string isbn);
}