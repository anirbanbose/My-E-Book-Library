using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEbookLibrary.Api.ActionFilters;
using MyEbookLibrary.Common;
using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Author;
using MyEbookLibrary.Common.DTO.AuthorType;
using MyEbookLibrary.Common.DTO.Responses;
using MyEbookLibrary.Service.Contracts;
using MyEbookLibrary.Service.Services;

namespace MyEbookLibrary.Api.Controllers
{
    [Route("api/authortypes")]
    [ApiController]
    public class AuthorTypeController : ControllerBase
    {

        private readonly ILogger<AuthorTypeController> _logger;
        private readonly IAuthService _authService;
        private readonly IAuthorTypeService _authorTypeService;

        public AuthorTypeController(IAuthorTypeService authorTypeService, ILogger<AuthorTypeController> logger, IAuthService authService)
        {
            _authorTypeService = authorTypeService;
            _logger = logger;
            _authService = authService;
        }

        [HttpGet("authortypedropdownlist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DropdownResult<AuthorTypeDropdownListDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetAuthorTypeDropdownList()
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var authorTypeList = await _authorTypeService.GetAuthorTypeDropdownList(user!);

            return Results.Ok(authorTypeList);
        }

    }
}
