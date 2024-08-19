using AdeLankaBackEnd.DataTransferObjects;
using AdeLankaBackEnd.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeLankaBackEnd.MyUtilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Document, DocumentResponseDto>();

            CreateMap<Comment, CommentResponseDto>()
                .ForMember(src => src.CommentCreatedBy,
                           opt => opt.MapFrom(src => src.NoteUser.FirstName + " " + src.NoteUser.LastName))
                .ForMember(src => src.Content,
                           opt => opt.MapFrom(src => src.Content))
                .ForMember(src => src.DateCommented,
                           opt => opt.MapFrom(src => src.DateCommented))
                .ForMember(src => src.NoteTitle,
                           opt => opt.MapFrom(src => src.Note.Title));

            CreateMap<Note, NoteResponseDto>()
                .ForMember(src => src.Title,
                           opt => opt.MapFrom(src => src.Title))
                .ForMember(src => src.CreatedBy,
                           opt => opt.MapFrom(src => src.NoteUser.FirstName+" "+ src.NoteUser.LastName))
                .ForMember(src => src.Content,
                           opt => opt.MapFrom(src => src.Content))
                .ForMember(src => src.DateCreated,
                           opt => opt.MapFrom(src => src.DateCreated))
                .ForMember(src => src.Comments,
                           opt => opt.MapFrom(src => src.Comments));

            
        }
    }
}
