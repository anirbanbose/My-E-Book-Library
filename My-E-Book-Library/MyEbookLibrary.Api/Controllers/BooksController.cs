using Microsoft.AspNetCore.Mvc;
using MyEbookLibrary.Service.Contracts;
using Microsoft.AspNetCore.Authorization;
using MyEbookLibrary.Common.Enums;
using MyEbookLibrary.Common.DTO.Book;
using MyEbookLibrary.Common.DTO.Requests;
using MyEbookLibrary.Common.DTO.Responses;
using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Api.ActionFilters;
using MyEbookLibrary.Common;

namespace MyEbookLibrary.Api.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly ILogger<BooksController> _logger;
        private readonly IAuthService _authService;
        private readonly IBookService _bookService;
        public BooksController(ILogger<BooksController> logger, IAuthService authService, IBookService bookService)
        {
            _logger = logger;
            _authService = authService;
            _bookService = bookService;
        }

        [HttpGet("booklist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListResult<BookListDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetBookList(int pageNumber, int pageSize, OrderByEnum sortOrder, string searchText = "", string sortColumn = "", int? authorId = null, int? publisherId = null, int? genreId = null, int? languageId = null)
        {
            if (pageSize <= 0 || pageNumber < 0)
            {
                return Results.BadRequest();
            }

            BookSearchRequestDTO listRequest = new BookSearchRequestDTO(pageNumber, pageSize, searchText, sortColumn, sortOrder, authorId, publisherId, genreId, languageId);
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var bookList = await _bookService.GetBookList(listRequest, user!);

            return Results.Ok(bookList);
        }

        [HttpPost("savebook")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize()]
        public async Task<IResult> SaveBook([FromBody] SaveBookDTO book)
        {
            if (book.Files == null || book.Files.Count == 0)
            {
                return Results.BadRequest("No files uploaded.");
            }

            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var result = await _bookService.SaveBook(book, user!);
            
            return Results.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailResult<BookDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetBook(int bookId)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var book = await _bookService.GetBook(bookId, user!);
            if (book == null)
            {
                return Results.BadRequest();
            }
            return Results.Ok(book);
        }


        [HttpGet("bookcopies")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailResult<BookCopyDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetBookCopyList(int bookId)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var book = await _bookService.GetBookCopiesByBookId(bookId, user!);
            
            return Results.Ok(book);
        }

        [HttpGet("download/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailResult<FileDataDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> Download(int id)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var file = await _bookService.GetFileForDownlaod(id, user!);
            
            return Results.Ok(file); 
        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> DeleteBook(int bookId)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var result = await _bookService.DeleteBook(bookId, user!);

            return Results.Ok(result);
        }

        [HttpGet("bookdetail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailResult<BookDetailDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetBookDetail(int bookId)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var book = await _bookService.GetBookDetail(bookId, user!);
            if (book == null)
            {
                return Results.BadRequest();
            }
            return Results.Ok(book);
        }
    }
}
