using BookAuthorApi.Domain.Entities;
using BookAuthorApi.Domain.Interfaces;
using BookAuthorApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookAuthorApi.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books.Include(b => b.Author).ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        return await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Book> AddAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Books.AnyAsync(b => b.Id == id);
    }

    public async Task<bool> IsbnExistsAsync(string isbn)
    {
        return await _context.Books.AnyAsync(b => b.Isbn == isbn);
    }
}