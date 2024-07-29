using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyEbookLibrary.Common;
using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Book;
using MyEbookLibrary.Common.DTO.BookAuthor;
using MyEbookLibrary.Common.DTO.Genre;
using MyEbookLibrary.Common.DTO.Language;
using MyEbookLibrary.Common.DTO.Requests;
using MyEbookLibrary.Common.DTO.Responses;
using MyEbookLibrary.Common.DTO.Publisher;
using MyEbookLibrary.Data.EF.Entities;
using MyEbookLibrary.Data.Repository.Contracts;
using MyEbookLibrary.Service.Contracts;
using System.Drawing;
using MyEbookLibrary.Data.Repository.Specifications;
using System.Linq.Expressions;
using System.Globalization;

namespace MyEbookLibrary.Service.Services
{
    public class BookService : IBookService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IBookRepository _bookRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IBookCopyRepository _bookCopyRepository;

        private readonly ILogger<BookService> _logger;

        public BookService(IMapper mapper, IConfiguration configuration, IBookRepository bookRepository, ILogger<BookService> logger, IPublisherRepository publisherRepository, IAuthorRepository authorRepository, IGenreRepository genreRepository, ILanguageRepository languageRepository, IBookCopyRepository bookCopyRepository)
        {
            _mapper = mapper;
            _configuration = configuration;
            _bookRepository = bookRepository;
            _logger = logger;
            _publisherRepository = publisherRepository;
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;
            _languageRepository = languageRepository;
            _bookCopyRepository = bookCopyRepository;
        }

        public async Task<ListResult<BookListDTO>> GetBookList(BookSearchRequestDTO listRequest, UserDTO user)
        {
            if (listRequest != null)
            {
                Expression<Func<Book, bool>> criteria = CreateBookListCriteria(listRequest, user);
                var books = await _bookRepository.GetByCriteriaAsync(criteria);
                if (books != null && books.Count > 0)
                {
                    int totalCount = books.Count;
                    if (totalCount > 0 && totalCount > listRequest.PageSize * listRequest.PageNumber)
                    {
                        List<BookListDTO> list = new List<BookListDTO>();
                        foreach (var book in books)
                        {
                            list.Add(new BookListDTO(book.Id, book.Title, book.BookImage, book.Publisher?.PublisherName, Helper.ListToCommaSeparatedString(book.Genres.Select(d => d.GenreName).OrderBy(d => d)), Helper.ListToCommaSeparatedString(book.BookAuthors.Select(d => $"{d.Author.FirstName}{(!string.IsNullOrEmpty(d.Author.MiddleName) ? " " + d.Author.MiddleName : "")} {d.Author.LastName}{((book.BookAuthors.Count > 1 && d.AuthorType != null && d.AuthorType.Id == 1) ? " (" + d.AuthorType?.TypeName + ")" : "")}").OrderBy(d => d)), Helper.ListToCommaSeparatedString(book.Languages.Select(d => d.LanguageName).OrderBy(d => d)), book.Copies.Count));
                        }
                        var records = Helper.SortAndPage(list, listRequest.PageSize, listRequest.PageNumber, string.IsNullOrEmpty(listRequest.SortColumnName) ? "Title" : listRequest.SortColumnName, listRequest.SortBy);
                        return ListResult<BookListDTO>.Success(records, totalCount);

                    }
                }
            }
            return ListResult<BookListDTO>.Failure(Error.RecordNotFound()); 
        }

        private static Expression<Func<Book, bool>> CreateBookListCriteria(BookSearchRequestDTO listRequest, UserDTO user)
        {
            string searchText = !string.IsNullOrEmpty(listRequest.SearchText) ? listRequest.SearchText.Trim().ToLower() : string.Empty;
            return book => !book.Deleted && book.AddedBy == user.Id
                                                    &&
                                                    (string.IsNullOrEmpty(searchText) ||
                                                    book.Title.ToLower().Contains(searchText) ||
                                                    (book.Publisher != null && book.Publisher.PublisherName.ToLower().Contains(searchText)) ||
                                                    (book.BookAuthors.Select(ba => ba.Author).Count() > 0 && book.BookAuthors.Select(ba => ba.Author).Where(a => !a.Deleted).Any(a => a.FirstName.ToLower().Contains(searchText) || a.LastName.ToLower().Contains(searchText) || (!string.IsNullOrEmpty(a.MiddleName) && a.MiddleName.ToLower().Contains(searchText)))) ||
                                                    (book.Languages.Count > 0 && book.Languages.Any(l => l.LanguageName.ToLower().Contains(searchText))) ||
                                                    (book.Genres.Count > 0 && book.Genres.Any(g => g.GenreName.ToLower().Contains(searchText)))
                                                    )
                                                    &&
                                                    (
                                                        (!listRequest.AuthorId.HasValue && !listRequest.PublisherId.HasValue && !listRequest.GenreId.HasValue && !listRequest.LanguageId.HasValue)
                                                        ||
                                                        (
                                                            listRequest.AuthorId.HasValue &&
                                                            book.BookAuthors.Select(ba => ba.Author).Any(a => a.Id == listRequest.AuthorId.Value && !a.Deleted)
                                                        )
                                                        ||
                                                        (
                                                            listRequest.PublisherId.HasValue &&
                                                            book.PublisherId == listRequest.PublisherId.Value
                                                        )
                                                        ||
                                                        (
                                                            listRequest.GenreId.HasValue &&
                                                            book.Genres.Any(d => d.Id == listRequest.GenreId.Value && !d.Deleted)
                                                        )
                                                        ||
                                                        (
                                                            listRequest.LanguageId.HasValue &&
                                                            book.Languages.Any(d => d.Id == listRequest.LanguageId.Value && !d.Deleted)
                                                        )
                                                    );
        }

        public async Task<DetailResult<BookDTO>> GetBook(int id, UserDTO user)
        {
            try
            {
                if (id > 0)
                {
                    var book = await _bookRepository.GetAsync(id, d => d.AddedBy == user.Id);
                    if (book != null)
                    {

                        var dto = new BookDTO()
                        {
                            Id = book.Id,
                            Title = book.Title,
                            BookImage = book.BookImage,
                            Description = book.Description,
                            Subject = book.Subject,
                            ISBN13 = book.ISBN13,
                            ISBN10 = book.ISBN10,
                            EditionName = book.EditionName,
                            NoOfPages = book.NoOfPages,
                            PublishedDate =  book.PublishedDate,
                            Authors = book.BookAuthors.Count > 0 && book.BookAuthors.Select(a => a.Author).Count() > 0 ?
                                            new List<BookAuthorDTO>(book.BookAuthors.Select(d => new BookAuthorDTO() { Id = d.Author.Id, AuthorName = $"{d.Author.FirstName}{(!string.IsNullOrEmpty(d.Author.MiddleName) ? " " + d.Author.MiddleName : "")} {d.Author.LastName}", AuthorTypeId = d.AuthorTypeId }))
                                            : new List<BookAuthorDTO>(),
                            Genres = book.Genres.Count > 0 ?
                                            new List<GenreDTO>(book.Genres.Select(d => new GenreDTO(d.Id, d.GenreName)))
                                            : new List<GenreDTO>(),
                            Languages = book.Languages.Count > 0 ?
                                            new List<LanguageDTO>(book.Languages.Select(d => new LanguageDTO(d.Id, d.LanguageName)))
                                            : new List<LanguageDTO>(),
                            Publisher = book.Publisher != null ? new PublisherDTO(book.Publisher.Id, book.Publisher.PublisherName) : null,
                            Files = book.Copies.Select(d => new FileDataDto() { Id = d.Id, FileName = d.FileName, FileType = d.FileType, FileSize = d.FileSize }).ToList(),


                        };
                        return DetailResult<BookDTO>.Success(dto);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving the author detail for id - {0} by user id - {1}", id, user.Id);
            }
            return DetailResult<BookDTO>.Failure(Error.RecordNotFound());
        }

        public async Task<SaveResult> SaveBook(SaveBookDTO book, UserDTO user)
        {
            if (book == null)
            {
                return SaveResult.Failure(Error.ValidationError("Null Book argument passed for saving."));
            }

            if (book.Files == null || book.Files.Count == 0)
            {
                return SaveResult.Failure(Error.ValidationError("No Ebook uploaded."));
            }
            try
            {
                DateTime dateTime = DateTime.UtcNow;
                Book? entity = null;
                if (book != null)
                {
                    if (book.Id > 0)
                    {
                        entity = await _bookRepository.GetAsync(book.Id, d => d.AddedBy == user.Id); 
                        if (entity != null)
                        {
                            await ProcessBookUpdate(book, entity, user.Id, dateTime);
                        }
                        else
                        {
                            return SaveResult.Failure(Error.ValidationError("No book found to save."));
                        }
                    }
                    else
                    {
                        entity = await AddNewBook(book, user.Id, dateTime);
                    }
                    

                    if (entity.Id == 0)
                    {
                        entity = await _bookRepository.AddAsync(entity);
                    }
                    else
                    {
                        entity = await _bookRepository.UpdateAsync(entity);
                    }

                    return SaveResult.Success();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while saving the book with id - {0} by user id - {1}", book.Id, user.Id);
            }
            return SaveResult.Failure(Error.SaveFailure("Book couldn't be saved."));
        }

        public async Task<DetailResult<BookCopyDTO>> GetBookCopiesByBookId(int bookId, UserDTO user)
        {
            Specification<Book> specification = new Specification<Book>();
            specification.AddCriteria(d => !d.Deleted && d.AddedBy == user.Id);
            specification.AddIncludes([d => d.Copies.Where(c => !c.Deleted)]);
            var book = await _bookRepository.GetAsync(bookId, specification);
            if(book == null)
            {
                return DetailResult<BookCopyDTO>.Failure(Error.RecordNotFound());
            }
            var result = new BookCopyDTO(book.Title);
            foreach (var c in book.Copies)
            {
                result.Copies.Add(new FileDataDto { Id = c.Id, FileName = c.FileName, FileType = c.FileType, FileSize = c.FileSize });
            }
            return DetailResult<BookCopyDTO>.Success(result);
        }

        public async Task<DetailResult<FileDataDto>> GetFileForDownlaod(int bookCopyId, UserDTO user)
        {
            var bookcopy = await _bookCopyRepository.GetAsync(bookCopyId, d => d.AddedBy == user.Id && !d.Deleted);
            if (bookcopy == null)
            {
                return DetailResult<FileDataDto>.Failure(Error.RecordNotFound());
            }
            var result = new FileDataDto()
            {
                FileName = bookcopy.FileName,
                FileType = bookcopy.FileType,
                FileData = Convert.ToBase64String(bookcopy.BinaryFile)
            };
            return DetailResult<FileDataDto>.Success(result);
        }

        public async Task<bool> DeleteBook(int id, UserDTO user)
        {
            bool retVal = false;
            try
            {
                if (id > 0)
                {
                    var book = await _bookRepository.GetAsync(id, d => d.AddedBy == user.Id);
                    if (book != null)
                    {
                        await _bookRepository.DeleteAsync(id);
                        retVal = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting the book with id - {0} by user id - {1}", id, user.Id);
            }

            return retVal;
        }

        public async Task<DetailResult<BookDetailDTO>> GetBookDetail(int id, UserDTO user)
        {
            try
            {
                if (id > 0)
                {
                    var book = await _bookRepository.GetAsync(id, d => d.AddedBy == user.Id);
                    if (book != null)
                    {

                        var dto = new BookDetailDTO()
                        {
                            Id = book.Id,
                            Title = book.Title,
                            BookImage = book.BookImage,
                            Description = book.Description,
                            Subject = book.Subject,
                            ISBN13 = book.ISBN13,
                            ISBN10 = book.ISBN10,
                            EditionName = book.EditionName,
                            NoOfPages = book.NoOfPages,
                            PublishedDate = book.PublishedDate?.ToString("dd MMM yyyy"),
                            Authors = Helper.ListToCommaSeparatedString(book.BookAuthors.OrderBy(d => d.AuthorTypeId).Select(d => $"{d.Author.FirstName}{(!string.IsNullOrEmpty(d.Author.MiddleName) ? " " + d.Author.MiddleName : "")} {d.Author.LastName}{((book.BookAuthors.Count > 1 && d.AuthorType != null) ? " (" + d.AuthorType?.TypeName + ")" : "")}")),
                            Genres = Helper.ListToCommaSeparatedString(book.Genres.Select(d => d.GenreName).OrderBy(d => d)),
                            Languages = Helper.ListToCommaSeparatedString(book.Languages.Select(d => d.LanguageName).OrderBy(d => d)),
                            Publisher = book.Publisher?.PublisherName,
                            Files = book.Copies.Select(d => new FileDetailDTO() { Id = d.Id, FileName = d.FileName, FileType = d.FileType, FileSize = d.FileSize }).ToList(),


                        };
                        return DetailResult<BookDetailDTO>.Success(dto);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving the author detail for id - {0} by user id - {1}", id, user.Id);
            }
            return DetailResult<BookDetailDTO>.Failure(Error.RecordNotFound());
        }

        #region "private methods"
        private async Task<Book> AddNewBook(SaveBookDTO book, int userId, DateTime dateTime)
        {
            Book entity = new Book(book.Title);
            if (book.ImageUploaded && !string.IsNullOrEmpty(book.BookImage))
            {
                if (Helper.IsWindowsOs)
                {
                    entity.BookImage = ResizeImage(book.BookImage);
                }
                else
                {
                    entity.BookImage = book.BookImage;
                }
            }
            entity.Subject = book.Subject;
            entity.ISBN13 = book.ISBN13;
            entity.ISBN10 = book.ISBN10;
            entity.Description = book.Description;
            entity.PublishedDate = !string.IsNullOrEmpty(book.PublishedDate) ? DateTime.ParseExact(book.PublishedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) : null; 
            entity.EditionName = !string.IsNullOrEmpty(book.EditionName) ? book.EditionName : null;
            entity.NoOfPages = book.NoOfPages.HasValue ? book.NoOfPages.Value : null;

            if (book.Publisher != null)
            {
                await AddPublisher(book.Publisher.Id, userId, entity);
            }
            if (book.Authors != null)
            {
                await AddAuthors(book.Authors, userId, entity);
            }
            if (book.Genres != null)
            {
                await AddGenres(book.Genres.Select(g => g.Id), userId, entity);
            }
            if (book.Languages != null)
            {
                await AddLanguages(book.Languages.Select(l => l.Id), userId, entity);
            }

            foreach (var file in book.Files)
            {
                if (!string.IsNullOrEmpty(file.FileData))
                {
                    entity.Copies.Add(GetBookCopy(file.FileName, Convert.FromBase64String(file.FileData), file.FileType, file.FileSize, userId, dateTime));
                }
            }

            entity.AddedBy = userId;
            entity.AddedDate = dateTime;
            entity.LastUpdatedDate = dateTime;
            return entity;
        }

        private string? ResizeImage(string image)
        {
            var array = image.Split(',');
            if (array.Length == 2)
            {
                string firstPart = array[0];
                string base64Part = array[1];
                return "data:image/jpeg;base64," + Convert.ToBase64String(Helper.ResizeImage(Convert.FromBase64String(base64Part), new System.Drawing.Size(150, 200)));
            }
            return null;
        }

        private async Task AddPublisher(int publisherId, int userId, Book entity)
        {
            if (publisherId > 0)
            {
                Publisher? publiser = await _publisherRepository.GetAsync(publisherId, d => (d.AddedBy == userId || d.AddedBy == Constants.SYSTEM_USER_ID) && !d.Deleted);
                if (publiser != null)
                {
                    entity.Publisher = publiser;
                }
            }            
        }

        private async Task AddAuthors(IEnumerable<BookAuthorDTO> bookAuthors, int userId, Book entity)
        {            
            var authors = await _authorRepository.GetByCriteriaAsync(d => bookAuthors.Select(a => a.Id).Contains(d.Id) && (d.AddedBy == userId || d.AddedBy == Constants.SYSTEM_USER_ID) && !d.Deleted);
            if (authors.Count > 0)
            {
                foreach (var author in authors)
                {
                    var dto = bookAuthors.FirstOrDefault(d => d.Id == author.Id);
                    if(dto != null)
                    {
                        BookAuthor bookAuthor = new BookAuthor();
                        bookAuthor.AuthorId = author.Id;
                        bookAuthor.AuthorTypeId = dto.AuthorTypeId;

                        entity.BookAuthors.Add(bookAuthor);
                    }
                    
                }
            }
        }

        private async Task AddGenres(IEnumerable<int> genreIds, int userId, Book entity)
        {
            Specification<Genre> specification = new Specification<Genre>();
            specification.AddCriteria(d => genreIds.Contains(d.Id) && (d.AddedBy == userId || d.AddedBy == Constants.SYSTEM_USER_ID) && !d.Deleted);
            var genres = await _genreRepository.GetBySpecificationAsync(specification);
            if (genres != null)
            {
                foreach (var genre in genres)
                {
                    entity.Genres.Add(genre);
                }
            }
        }

        private async Task AddLanguages(IEnumerable<int> laguageIds, int userId, Book entity)
        {
            Specification<Language> specification = new Specification<Language>();
            specification.AddCriteria(d => laguageIds.Contains(d.Id) && (d.AddedBy == userId || d.AddedBy == Constants.SYSTEM_USER_ID) && !d.Deleted);
            var languages = await _languageRepository.GetBySpecificationAsync(specification);
            if (languages != null)
            {
                foreach (var language in languages)
                {
                    entity.Languages.Add(language);
                }
            }
        }


        private BookCopy GetBookCopy(string fileName, byte[] fileData, string fileType, int fileSize, int addedBy, DateTime addedDate)
        {
            return new BookCopy(addedBy, addedDate, addedDate)
            {
                FileName = fileName,
                BinaryFile = fileData,
                FileType = fileType,
                FileSize = fileSize,
            };
        }

        private async Task ProcessBookUpdate(SaveBookDTO book, Book entity, int userId, DateTime dateTime)
        {
            entity.Title = book.Title;
            if (book.ImageUploaded && !string.IsNullOrEmpty(book.BookImage))
            {
                if(Helper.IsWindowsOs)
                {
                    entity.BookImage = ResizeImage(book.BookImage);
                }
                else
                {
                    entity.BookImage = book.BookImage;
                }
                
            }
            else if(string.IsNullOrEmpty(book.BookImage))
            {
                entity.BookImage = null;
            }
            entity.Subject = book.Subject;
            entity.ISBN13 = book.ISBN13;
            entity.ISBN10 = book.ISBN10;
            entity.Description = book.Description;
            entity.EditionName = book.EditionName;
            entity.NoOfPages = book.NoOfPages;
            entity.PublishedDate = !string.IsNullOrEmpty(book.PublishedDate) ? DateTime.ParseExact(book.PublishedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) : null; 
            entity.LastUpdatedDate = dateTime;

            await ProcessBookAuthors(book, entity, userId);
            await ProcessPublisher(book, entity, userId);
            await ProcessGenres(book, entity, userId);
            await ProcessLanguages(book, entity, userId);

            ProcessFiles(book, entity, userId, dateTime);
        }

        private async Task ProcessGenres(SaveBookDTO book, Book entity, int userId)
        {
            var existingGenres = entity.Genres;
            var genreIds = book.Genres.Select(d => d.Id);

            var deletedGenres = existingGenres.Where(d => !genreIds.Contains(d.Id));
            var addedGenres = genreIds.Where(d => !existingGenres.Select(g => g.Id).Contains(d));

            foreach (var genre in deletedGenres)
            {
                entity.Genres.Remove(genre);
            }
            await AddGenres(addedGenres, userId, entity);
        }

        private async Task ProcessLanguages(SaveBookDTO book, Book entity, int userId)
        {
            var existingLanguages = entity.Languages;
            var languageIds = book.Languages.Select(d => d.Id);

            var deletedLanguages = existingLanguages.Where(d => !languageIds.Contains(d.Id));
            var addedLanguages = languageIds.Where(d => !existingLanguages.Select(l => l.Id).Contains(d));

            foreach (var language in deletedLanguages)
            {
                entity.Languages.Remove(language);
            }
            await AddLanguages(addedLanguages, userId, entity);
        }

        private async Task ProcessPublisher(SaveBookDTO book, Book entity, int userId)
        {
            if (book.Publisher != null)
            {
                if (book.Publisher.Id != entity.Publisher?.Id)
                {
                    await AddPublisher(book.Publisher.Id, userId, entity);
                }
            }
            else
            {
                entity.Publisher = null;
            }
        }


        private async Task ProcessBookAuthors(SaveBookDTO book, Book entity, int userId)
        {
            var existingBookAuthors = entity.BookAuthors;
            var bookAuthors = book.Authors;

            var deletedBookAuthors = existingBookAuthors.Where(d => !bookAuthors.Select(a => a.Id).Contains(d.AuthorId));
            var addedBookAuthors = bookAuthors.Where(d => !existingBookAuthors.Select(a => a.AuthorId).Contains(d.Id)).ToList();
            var modifedBookauthors = existingBookAuthors.Where(d => bookAuthors.Any(a => a.Id == d.AuthorId && a.AuthorTypeId != d.AuthorTypeId));
            foreach (var bookAuthor in deletedBookAuthors)
            {
                entity.BookAuthors.Remove(bookAuthor);
            }
            foreach (var bookAuthor in modifedBookauthors)
            {
                var modifiedAuthor = bookAuthors.FirstOrDefault(d => d.Id == bookAuthor.AuthorId);
                if (modifiedAuthor != null)
                {
                    addedBookAuthors.Add(modifiedAuthor);
                }
                entity.BookAuthors.Remove(bookAuthor);                
            }
            await AddAuthors(addedBookAuthors, userId, entity);
            
        }
        private void ProcessFiles(SaveBookDTO book, Book entity, int userId, DateTime dateTime)
        {
            if (book.Files != null && book.Files.Count > 0)
            {
                var addedFiles = book.Files.Where(d => d.Id == 0);
                var oldFiles = book.Files.Where(d => d.Id > 0);

                var existingFiles = entity.Copies;
                var deletedFiles = existingFiles.Where(d => !oldFiles.Select(f => f.Id).Contains(d.Id));
                foreach (var file in deletedFiles)
                {
                    entity.Copies.Remove(file);
                }

                foreach (var file in addedFiles)
                {
                    if (!string.IsNullOrEmpty(file.FileData))
                    {
                        entity.Copies.Add(GetBookCopy(file.FileName, Convert.FromBase64String(file.FileData), file.FileType, file.FileSize, userId, dateTime));
                    }
                }
            }
        }



        #endregion

    }
}
