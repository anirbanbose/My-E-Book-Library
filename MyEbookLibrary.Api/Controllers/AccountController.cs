using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEbookLibrary.Api.ActionFilters;
using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Responses;
using MyEbookLibrary.Service.Contracts;
using MyEbookLibrary.Service.Services;

namespace MyEbookLibrary.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;

        public AccountController(IUserService userService, ILogger<AccountController> logger) 
        {
            _logger = logger;
            _userService = userService;
        }


        [HttpGet("emailavailable")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IResult> EmailAvailable(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Results.BadRequest();
            }
            var result = await _userService.IsEmailAvailable(email.Trim());            
            return Results.Ok(result);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IResult> Register([FromBody] RegistrationDTO registrationDto)
        {            
            var result = await _userService.Register(registrationDto);
            if (!result.IsSuccess)
            {
                return Results.BadRequest();
            }

            return Results.Ok(result);
        }
    }
}
