// Mappings/MappingProfile.cs
using AutoMapper;
using backend.Models;
using backend.Dtos;
using Microsoft.EntityFrameworkCore.Infrastructure;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<backend.Models.File, FileDto>();
        CreateMap<FileDto, backend.Models.File>();
        CreateMap<Embedding, EmbeddingDto>();
        CreateMap<EmbeddingDto, Embedding>();
    }
}