using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEbookLibrary.Api.ActionFilters;
using MyEbookLibrary.Common;
using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Author;
using MyEbookLibrary.Common.DTO.Requests;
using MyEbookLibrary.Common.DTO.Responses;
using MyEbookLibrary.Common.Enums;
using MyEbookLibrary.Service.Contracts;
using MyEbookLibrary.Service.Services;

namespace MyEbookLibrary.Api.Controllers
{
    [Route("api/author")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ILogger<AuthorsController> _logger;
        private readonly IAuthService _authService;
        private readonly IAuthorService _authorService;

        public AuthorsController(ILogger<AuthorsController> logger, IAuthService authService, IAuthorService authorService)
        {
            _logger = logger;
            _authService = authService;
            _authorService = authorService;
        }

        [HttpGet("authorlist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListResult<AuthorListDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetAuthorList(int pageNumber, int pageSize, OrderByEnum sortOrder, string searchText = "", string sortColumn = "")
        {
            if (pageSize <= 0 || pageNumber < 0)
            {
                return Results.BadRequest();
            }

            ListRequestDTO listRequest = new ListRequestDTO(pageNumber, pageSize, searchText, sortColumn, sortOrder);
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var authorList = await _authorService.GetAuthorList(listRequest, user!);

            return Results.Ok(authorList);
        }

        [HttpPost("saveauthor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveResult<AuthorDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize()]
        public async Task<IResult> SaveAuthor([FromBody] AuthorDTO author)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var result = await _authorService.SaveAuthor(author, user!);
            if (!result.IsSuccess)
            {
                return Results.BadRequest();
            }
            return Results.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailResult<AuthorDetailDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetAuthor(int authorId)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var author = await _authorService.GetAuthor(authorId, user!);
            if (author == null)
            {
                return Results.BadRequest();
            }
            return Results.Ok(author);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> DeleteAuthor(int authorId)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var author = await _authorService.DeleteAuthor(authorId, user!);

            return Results.Ok(author);
        }

        [HttpGet("authordropdownlist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DropdownResult<AuthorDropdownListDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetAuthorDropdownList(string? q)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var authorList = await _authorService.GetAuthorDropdownList(q, user!);

            return Results.Ok(authorList);
        }
    }
}
