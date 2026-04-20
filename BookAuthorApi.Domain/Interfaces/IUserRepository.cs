using BookAuthorApi.Domain.Entities;

namespace BookAuthorApi.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
}