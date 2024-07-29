using MailKit.Search;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyEbookLibrary.Common;
using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Dashboard;
using MyEbookLibrary.Common.DTO.Responses;
using MyEbookLibrary.Data.EF.Entities;
using MyEbookLibrary.Data.Repository.Contracts;
using MyEbookLibrary.Data.Repository.Repositories;
using MyEbookLibrary.Data.Repository.Specifications;
using MyEbookLibrary.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Service.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthorRepository _authorRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly ILogger<BookService> _logger;
        public DashboardService(IConfiguration configuration, IAuthorRepository authorRepository, IGenreRepository genreRepository, ILanguageRepository languageRepository, IPublisherRepository publisherRepository, ILogger<BookService> logger)
        {
            _configuration = configuration;
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;
            _publisherRepository = publisherRepository;
            _languageRepository = languageRepository;
            _logger = logger;
        }

        public async Task<ListResult<AuthorListBookCountDTO>> GetAuthorList(UserDTO user)
        {
            int recordLimit = Convert.ToInt32(_configuration["DashboardListRecordLimit"]);
            Expression<Func<Author, bool>> criteria = author =>
                                        (author.AddedBy == user.Id || author.AddedBy == Constants.SYSTEM_USER_ID) && !author.Deleted && author.BookAuthors.Select(d => d.Book).Where(d => !d.Deleted).Count() > 0;


            var authors = await _authorRepository.GetByCriteriaAsync(criteria);
            List<AuthorListBookCountDTO> list = authors.OrderByDescending(d => d.BookAuthors.Select(ba => ba.Book).Where(d => !d.Deleted).Count()).Select(a => new AuthorListBookCountDTO()
            {
                AuthorId = a.Id,
                AuthorName = $"{a.FirstName}{(!string.IsNullOrEmpty(a.MiddleName) ? " " + a.MiddleName : "")} {a.LastName}",
                BookCount = a.BookAuthors.Select(d => d.Book).Where(d => !d.Deleted).Count()
            }).Take(recordLimit).ToList();
            return ListResult<AuthorListBookCountDTO>.Success(list, recordLimit);
        }

        public async Task<ListResult<GenreListBookCountDTO>> GetGenreList(UserDTO user)
        {
            int recordLimit = Convert.ToInt32(_configuration["DashboardListRecordLimit"]);
            Specification<Genre> specification = new Specification<Genre>();
            specification.AddCriteria(g => (g.AddedBy == user.Id || g.AddedBy == Constants.SYSTEM_USER_ID) && !g.Deleted && g.Books.Where(d => !d.Deleted).Count() > 0);
            specification.AddIncludes([g => g.Books.Where(d => !d.Deleted)]);
            specification.AddOrderByDesc(g => g.Books.Where(d => !d.Deleted).Count());


            var genres = await _genreRepository.GetBySpecificationAsync(specification, recordLimit);
            List<GenreListBookCountDTO> list = genres.Select(g => new GenreListBookCountDTO()
            {
                GenreId = g.Id,
                GenreName = g.GenreName,
                BookCount = g.Books.Where(d => !d.Deleted).Count()
            }).ToList();
            return ListResult<GenreListBookCountDTO>.Success(list, recordLimit);
        }

        public async Task<ListResult<PublisherListBookCountDTO>> GetPublisherList(UserDTO user)
        {
            int recordLimit = Convert.ToInt32(_configuration["DashboardListRecordLimit"]);
            Specification<Publisher> specification = new Specification<Publisher>();
            specification.AddCriteria(p => (p.AddedBy == user.Id || p.AddedBy == Constants.SYSTEM_USER_ID) && !p.Deleted && p.Books.Where(d => !d.Deleted).Count() > 0);
            specification.AddIncludes([p => p.Books.Where(d => !d.Deleted)]);
            specification.AddOrderByDesc(p => p.Books.Where(d => !d.Deleted).Count());


            var publishers = await _publisherRepository.GetBySpecificationAsync(specification, recordLimit);
            List<PublisherListBookCountDTO> list = publishers.Select(p => new PublisherListBookCountDTO()
            {
                PublisherId = p.Id,
                PublisherName = p.PublisherName,
                BookCount = p.Books.Where(d => !d.Deleted).Count()
            }).ToList();
            return ListResult<PublisherListBookCountDTO>.Success(list, recordLimit);
        }

        public async Task<ListResult<LanguageListBookCountDTO>> GetLanguageList(UserDTO user)
        {
            int recordLimit = Convert.ToInt32(_configuration["DashboardListRecordLimit"]);
            Specification<Language> specification = new Specification<Language>();
            specification.AddCriteria(l => (l.AddedBy == user.Id || l.AddedBy == Constants.SYSTEM_USER_ID) && !l.Deleted && l.Books.Where(d => !d.Deleted).Count() > 0);
            specification.AddIncludes([l => l.Books.Where(d => !d.Deleted)]);
            specification.AddOrderByDesc(l => l.Books.Where(d => !d.Deleted).Count());


            var languages = await _languageRepository.GetBySpecificationAsync(specification, recordLimit);
            List<LanguageListBookCountDTO> list = languages.Select(l => new LanguageListBookCountDTO()
            {
                LanguageId = l.Id,
                LanguageName = l.LanguageName,
                BookCount = l.Books.Where(d => !d.Deleted).Count()
            }).ToList();
            return ListResult<LanguageListBookCountDTO>.Success(list, recordLimit);
        }
    }
}
