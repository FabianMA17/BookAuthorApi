namespace BookAuthorApi.Application.DTOs;

public class UpdateBookDto
{
    public string? Title { get; set; }
    public string? Isbn { get; set; }
    public int? PublicationYear { get; set; }
    public Guid? AuthorId { get; set; }
    public string? CoverUrl { get; set; }
    
}