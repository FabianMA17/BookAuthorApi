using BookAuthorApi.Domain.Entities;
using BookAuthorApi.Domain.Interfaces;
using BookAuthorApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookAuthorApi.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
}