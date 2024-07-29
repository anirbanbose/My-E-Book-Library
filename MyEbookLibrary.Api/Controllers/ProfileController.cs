using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEbookLibrary.Common.DTO.AutheticationDTO;
using MyEbookLibrary.Data.EF.Entities;
using MyEbookLibrary.Service.Contracts;
using Microsoft.AspNetCore.Authorization;
using MyEbookLibrary.Common.DTO.Responses;
using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Api.ActionFilters;
using MyEbookLibrary.Common;

namespace MyEbookLibrary.Api.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public ProfileController(IUserService userService, ILogger<ProfileController> logger, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailResult<UserDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetProfile()
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var result = await _userService.GetProfile(user!.Id);            
            return Results.Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveResult<UserDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize()]
        public async Task<IResult> SaveProfile([FromBody] SaveProfileDTO model)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var result = await _userService.SaveProfile(model, user!.Id);
            if (!result.IsSuccess)
            {
                return Results.BadRequest();
            }
            return Results.Ok(result);
        }

        [HttpPost("changepassword")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize()]
        public async Task<IResult> ChangePassword([FromBody] ChangePasswordDTO passwordDto)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var result = await _userService.ChangePassword(passwordDto, user!.Id);
            if (!result.IsSuccess)
            {
                return Results.BadRequest();
            }
            return Results.Ok(result);
        }
    }
}
