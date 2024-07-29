using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEbookLibrary.Api.ActionFilters;
using MyEbookLibrary.Common;
using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Book;
using MyEbookLibrary.Common.DTO.Dashboard;
using MyEbookLibrary.Common.DTO.Requests;
using MyEbookLibrary.Common.DTO.Responses;
using MyEbookLibrary.Common.Enums;
using MyEbookLibrary.Service.Contracts;
using MyEbookLibrary.Service.Services;

namespace MyEbookLibrary.Api.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IAuthService _authService;
        private readonly IDashboardService _dashboardService;

        public DashboardController(ILogger<DashboardController> logger, IAuthService authService, IDashboardService dashboardService)
        {
            _logger = logger;
            _authService = authService;
            _dashboardService = dashboardService;
        }

        [HttpGet("authorlist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListResult<AuthorListBookCountDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetAuthorList()
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var authorList = await _dashboardService.GetAuthorList(user!);

            return Results.Ok(authorList);
        }

        [HttpGet("genrelist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListResult<GenreListBookCountDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetGenreList()
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var genreList = await _dashboardService.GetGenreList(user!);

            return Results.Ok(genreList);
        }

        [HttpGet("languagelist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListResult<LanguageListBookCountDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetLangaugeList()
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var languageList = await _dashboardService.GetLanguageList(user!);

            return Results.Ok(languageList);
        }

        [HttpGet("publisherlist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListResult<PublisherListBookCountDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetPublisherList()
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var publisherList = await _dashboardService.GetPublisherList(user!);

            return Results.Ok(publisherList);
        }

    }
}
