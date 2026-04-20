using AutoMapper;
using BookAuthorApi.Application.DTOs;
using BookAuthorApi.Domain.Entities;

namespace BookAuthorApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? src.Author.Name : string.Empty));
        CreateMap<CreateBookDto, Book>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
        CreateMap<UpdateBookDto, Book>();
        CreateMap<Author, AuthorDto>();
        CreateMap<CreateAuthorDto, Author>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
        CreateMap<UpdateAuthorDto, Author>();
    }
}