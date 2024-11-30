using AutoMapper;
using backend.Models;
using backend.Dtos;
using Microsoft.EntityFrameworkCore.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();

        // File mappings
        CreateMap<backend.Models.File, FileDto>();
        CreateMap<FileDto, backend.Models.File>();

        // Embedding mappings
        CreateMap<Embedding, EmbeddingDto>();
        CreateMap<EmbeddingDto, Embedding>();

        // Client mappings
        CreateMap<Client, ClientRequestDto>();
        CreateMap<ClientRequestDto, Client>();
        CreateMap<Client, ClientResponseDto>();
        CreateMap<ClientResponseDto, Client>();
        CreateMap<Case, CaseSummaryDto>();
        CreateMap<CaseSummaryDto, Case>();

        // Case mappings
        CreateMap<Case, CaseRequestDto>();
        CreateMap<CaseRequestDto, Case>();
        CreateMap<Case, CaseResponseDto>()
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Client))
            .ForMember(dest => dest.AssignedUser, opt => opt.MapFrom(src => src.AssignedUser))
            .ForMember(dest => dest.Documents, opt => opt.MapFrom(src => src.Documents));
        CreateMap<CaseResponseDto, Case>();

        // Document mappings
        CreateMap<Document, DocumentRequestDto>();
        CreateMap<DocumentRequestDto, Document>();
        CreateMap<Document, DocumentResponseDto>()
            .ForMember(dest => dest.Case, opt => opt.MapFrom(src => src.Case))
            .ForMember(dest => dest.File, opt => opt.MapFrom(src => src.File));
        CreateMap<DocumentResponseDto, Document>();
        CreateMap<backend.Models.File, FileSummaryDto>();
        CreateMap<FileSummaryDto, backend.Models.File>();

        // Additional mappings
        CreateMap<Client, ClientSummaryDto>();
        CreateMap<ClientSummaryDto, Client>();
        CreateMap<Document, DocumentSummaryDto>();
        CreateMap<DocumentSummaryDto, Document>();
    }
}
