using BookAuthorApi.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace BookAuthorApi.Infrastructure.Services;

public class BookCoverService : IBookCoverService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public BookCoverService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["BookCover:BaseUrl"] ?? "https://openlibrary.org/api/books";
    }

    public async Task<string?> GetBookCoverUrlAsync(string isbn)
    {
        try
        {
            var url = $"{_baseUrl}?bibkeys=ISBN:{isbn}&format=json";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonDocument.Parse(json);

            // The response format is: {"ISBN:978-3-16-148410-0": {"info_url": "...", "thumbnail_url": "..."}}
            var isbnKey = $"ISBN:{isbn}";
            if (data.RootElement.TryGetProperty(isbnKey, out var bookData) &&
                bookData.TryGetProperty("thumbnail_url", out var thumbnailUrl))
            {
                return thumbnailUrl.GetString();
            }

            return null;
        }
        catch
        {
            // Return null if there's any error with the external service
            return null;
        }
    }
}