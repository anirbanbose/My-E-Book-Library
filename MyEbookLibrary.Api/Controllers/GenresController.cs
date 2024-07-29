using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEbookLibrary.Api.ActionFilters;
using MyEbookLibrary.Common;
using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Genre;
using MyEbookLibrary.Common.DTO.Requests;
using MyEbookLibrary.Common.DTO.Responses;
using MyEbookLibrary.Common.Enums;
using MyEbookLibrary.Data.EF.Entities;
using MyEbookLibrary.Service.Contracts;
using MyEbookLibrary.Service.Services;

namespace MyEbookLibrary.Api.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ILogger<GenresController> _logger;
        private readonly IAuthService _authService;
        private readonly IGenreService _genreService;

        public GenresController(ILogger<GenresController> logger, IAuthService authService, IGenreService genreService)
        {
            _logger = logger;
            _authService = authService;
            _genreService = genreService;
        }

        [HttpGet("genrelist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListResult<GenreListDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetGenreList(int pageNumber, int pageSize, OrderByEnum sortOrder, string searchText = "", string sortColumn = "")
        {
            if (pageSize <= 0 || pageNumber < 0)
            {
                return Results.BadRequest();
            }

            ListRequestDTO listRequest = new ListRequestDTO(pageNumber, pageSize, searchText, sortColumn, sortOrder);
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var genreList = await _genreService.GetGenreList(listRequest, user!);

            return Results.Ok(genreList);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> DeleteGenre(int genreId)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var result = await _genreService.DeleteGenre(genreId, user!);
            if (!result)
            {
                return Results.BadRequest();
            }
            return Results.Ok();
        }

        [HttpGet("genredropdownlist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DropdownResult<GenreDropdownListDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetGenreDropdownList(string? q)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var genreList = await _genreService.GetGenreDropdownList(q, user!);

            return Results.Ok(genreList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailResult<GenreDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetGenre(int genreId)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var result = await _genreService.GetGenre(genreId, user!);
            if (!result.IsSuccess)
            {
                return Results.BadRequest();
            }
            return Results.Ok(result);
        }

        [HttpPost("savegenre")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveResult<GenreDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize()]
        public async Task<IResult> SaveGenre([FromBody] GenreDTO genre)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var result = await _genreService.SaveGenre(genre, user!);
            if (!result.IsSuccess)
            {
                return Results.BadRequest();
            }
            return Results.Ok(result);
        }
    }
}
