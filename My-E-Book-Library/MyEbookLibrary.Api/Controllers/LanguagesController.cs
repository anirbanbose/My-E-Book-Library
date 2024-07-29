using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MyEbookLibrary.Api.ActionFilters;
using MyEbookLibrary.Common;
using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Language;
using MyEbookLibrary.Common.DTO.Requests;
using MyEbookLibrary.Common.DTO.Responses;
using MyEbookLibrary.Common.Enums;
using MyEbookLibrary.Service.Contracts;
using MyEbookLibrary.Service.Services;

namespace MyEbookLibrary.Api.Controllers
{
    [Route("api/languages")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private readonly ILogger<LanguagesController> _logger;
        private readonly IAuthService _authService;
        private readonly ILanguageService _languageService;

        public LanguagesController(ILogger<LanguagesController> logger, IAuthService authService, ILanguageService languageService)
        {
            _logger = logger;
            _authService = authService;
            _languageService = languageService;
        }

        [HttpGet("languagelist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListResult<LanguageListDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetLanguageList(int pageNumber, int pageSize, OrderByEnum sortOrder, string searchText = "", string sortColumn = "")
        {
            if (pageSize <= 0 || pageNumber < 0)
            {
                return Results.BadRequest();
            }

            ListRequestDTO listRequest = new ListRequestDTO(pageNumber, pageSize, searchText, sortColumn, sortOrder);
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var languageList = await _languageService.GetLanguageList(listRequest, user!);

            return Results.Ok(languageList);
        }

        [HttpGet("languagedropdownlist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DropdownResult<LanguageDropdownListDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetLanguageDropdownList(string? q)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var languageList = await _languageService.GetLanguageDropdownList(q, user!);

            return Results.Ok(languageList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailResult<LanguageDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetLanguage(int languageId)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var result = await _languageService.GetLanguage(languageId, user!);
            if (!result.IsSuccess)
            {
                return Results.BadRequest();
            }
            return Results.Ok(result);
        }
    }
}
