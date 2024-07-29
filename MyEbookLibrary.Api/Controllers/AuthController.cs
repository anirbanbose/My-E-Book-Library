using MyEbookLibrary.Service.Contracts;
using MyEbookLibrary.Common.DTO;
using MyEbookLibrary.Common.DTO.AutheticationDTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MyEbookLibrary.Api.ActionFilters;

namespace MyEbookLibrary.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IResult> Login([FromBody] LoginDTO loginDTO)
        {
            var response = await _authService.Login(loginDTO.Email, loginDTO.Password, loginDTO.RememberMe, loginDTO.DeviceId, loginDTO.UserAgent);
            if (response != null && response.IsLoggedIn)
            {
                SetAuthenticationCookies(response, HttpContext);
                return Results.Ok(new
                {
                    response.User,
                    response.DeviceId,
                    IsLoggedIn = true
                });
            }
            return Results.Unauthorized();
        }


        [HttpPost("token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IResult> RefreshToken([FromBody] TokenDTO tokenDTO)
        {
            HttpContext.Request.Cookies.TryGetValue("accessToken", out var accessToken);
            HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);
            if (tokenDTO is null || string.IsNullOrEmpty(refreshToken))
            {
                return Results.BadRequest();
            }
            else
            {
                tokenDTO.RefreshToken = refreshToken;
            }
            if(!string.IsNullOrEmpty(accessToken))
            {
                tokenDTO.AccessToken = accessToken;
            }            

            var response = await _authService.RefreshToken(tokenDTO);
            if (response != null && response.IsLoggedIn)
            {
                SetAuthenticationCookies(response, HttpContext);
                return Results.Ok();
            }
            return Results.Unauthorized();
        }


        private void SetAuthenticationCookies(LoginResponse response, HttpContext context)
        {
            try
            {
                context.Response.Cookies.Append("accessToken", response.AccessToken,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMinutes(5),
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                });
                context.Response.Cookies.Append("refreshToken", response.RefreshToken,
                    new CookieOptions
                    {
                        Expires = response.RefreshTokenExpiry,
                        HttpOnly = true,
                        IsEssential = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while adding the Authentication cookies.");
            }
            
        }

    }
}
