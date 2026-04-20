namespace BookAuthorApi.Application.DTOs;

public class CreateBookDto
{
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public string CoverUrl { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public Guid AuthorId { get; set; }
}