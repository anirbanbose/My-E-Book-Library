using AutoMapper;
using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.AutheticationDTO;
using MyEbookLibrary.Common.DTO.Author;
using MyEbookLibrary.Common.DTO.Book;
using MyEbookLibrary.Common.DTO.Genre;
using MyEbookLibrary.Common.DTO.Language;
using MyEbookLibrary.Common.DTO.Publisher;
using MyEbookLibrary.Data.EF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Data.Repository.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Book, BookCopyDTO>().ReverseMap();
            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<Author, AuthorDetailDTO>().ReverseMap();
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<Language, LanguageDTO>().ReverseMap();
            CreateMap<Publisher, PublisherDTO>().ReverseMap();
            CreateMap<UserRefreshToken, UserRefreshTokenDTO>().ReverseMap();
        }
    }
}
