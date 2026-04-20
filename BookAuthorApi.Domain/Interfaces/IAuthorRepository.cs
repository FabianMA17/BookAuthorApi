using BookAuthorApi.Domain.Entities;

namespace BookAuthorApi.Domain.Interfaces;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAsync();
    Task<Author?> GetByIdAsync(Guid id);
    Task<Author> AddAsync(Author author);
    Task UpdateAsync(Author author);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}