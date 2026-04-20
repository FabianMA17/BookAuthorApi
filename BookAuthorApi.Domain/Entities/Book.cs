namespace BookAuthorApi.Domain.Entities;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public string CoverUrl { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public Guid AuthorId { get; set; }
    public Author? Author { get; set; }
}