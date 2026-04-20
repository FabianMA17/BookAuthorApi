using System.ComponentModel.DataAnnotations;

namespace BookAuthorApi.Application.DTOs;

public class CreateBooksDto
{
    [Required]
    public List<CreateBookDto> Books { get; set; } = new();
}