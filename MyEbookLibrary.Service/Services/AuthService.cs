using AutoMapper;
using MyEbookLibrary.Service.Contracts;
using MyEbookLibrary.Common.DTO.AutheticationDTO;
using MyEbookLibrary.Data.EF.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MyEbookLibrary.Data.Repository.Contracts;
using Microsoft.Extensions.Logging;
using MyEbookLibrary.Common.DTO.Account;

namespace MyEbookLibrary.Service.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ITokenRepository _tokenRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ILogger<AuthService> logger, UserManager<User> userManager, IMapper mapper, IConfiguration configuration, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _tokenRepository = tokenRepository;
            _logger = logger;
        }

        public async Task<LoginResponse> Login(string email, string password, bool rememberMe, string? deviceId, string userAgent)
        {
            _logger.LogInformation("Login process started for user - {0}.", email);
            LoginResponse response = new LoginResponse();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, password))
                    {
                        var userDTO = _mapper.Map<UserDTO>(user);
                        userDTO.Role = await GetUserRole(user);
                        string accessToken = GenerateAccessToken(userDTO);
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            response.AccessToken = accessToken;
                            response.RefreshToken = GenerateRefreshToken();
                            response.IsLoggedIn = true;
                            response.User = userDTO;
                            response.RefreshTokenExpiry = rememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(24);
                            response.DeviceId = !string.IsNullOrEmpty(deviceId) ? deviceId : Guid.NewGuid().ToString();
                            await SaveRefreshToken(response.RefreshToken, response.RefreshTokenExpiry.Value, user, response.DeviceId, userAgent);
                            _logger.LogInformation("Successfully Logged in user - {0}.", email);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while logging in user - " + email);
            }
            
            return response;
        }

        private async Task<UserRefreshTokenDTO> SaveRefreshToken(string refreshToken, DateTime refreshTokenExpiry, User user, string deviceId, string userAgent)
        {
            bool isNew = false;
            UserRefreshToken userRefreshToken = null;
            try
            {
                if (!string.IsNullOrEmpty(deviceId))
                {
                    userRefreshToken = await _tokenRepository.GetUserRefreshTokenAsync(user.Id, deviceId);
                }
                if (userRefreshToken == null)
                {
                    userRefreshToken = new UserRefreshToken();
                    userRefreshToken.UserId = user.Id;
                    isNew = true;
                }
                userRefreshToken.RefreshToken = refreshToken;
                userRefreshToken.CreatedAt = DateTime.UtcNow;
                userRefreshToken.ExpiresAt = refreshTokenExpiry;
                userRefreshToken.DeviceUniqueId = deviceId;
                userRefreshToken.UserAgent = userAgent;
                if (isNew)
                {
                    await _tokenRepository.AddUserRefreshTokenAsync(userRefreshToken);
                }
                else
                {
                    await _tokenRepository.UpdateUserRefreshTokenAsync(userRefreshToken);
                }

                return _mapper.Map<UserRefreshTokenDTO>(userRefreshToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while saving the Refresh Token for the user - {0}.", user.Email);
            }
            return null;            
        }

        private async Task<string> GetUserRole(User user)
        {
            try
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Count > 0)
                {
                    return roles[0];
                }
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, "Exception occurred while retrieving the role for the user - {0}", user.Email);
            }
            
            return string.Empty;
        }

        private string GenerateAccessToken(UserDTO user)
        {
            try
            {
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    expires: DateTime.Now.AddMinutes(2),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, "Exception occurred while generating the access token for the user - {0}.", user.Email);
            }
            return string.Empty;
            
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using(var numberGenerator = RandomNumberGenerator.Create())
            {
                numberGenerator.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetTokenPrincipal(string token)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var validation = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, validation, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken is null || 
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidOperationException("Invalid Token");
            }
            return principal;
        }

        public async Task<LoginResponse> RefreshToken(TokenDTO model)
        {
            LoginResponse response = new LoginResponse();
            try
            {
                string userEmail = null;
                if (!string.IsNullOrEmpty(model.AccessToken))
                {
                    var principal = GetTokenPrincipal(model.AccessToken);
                    if (principal?.Identity?.Name is null)
                    {
                        return response;
                    }
                    userEmail = principal?.Identity?.Name;
                }
                else
                {
                    userEmail = model.Email;
                }

                var user = await _userManager.FindByEmailAsync(userEmail);
                if (user is null || user.Deleted)
                {
                    return response;
                }
                var userRefreshToken = await _tokenRepository.GetUserRefreshTokenAsync(user.Id, model.DeviceId);

                if (userRefreshToken is null || userRefreshToken.RefreshToken != model.RefreshToken ||
                    userRefreshToken.ExpiresAt <= DateTime.UtcNow)
                {
                    return response;
                }

                var userDTO = _mapper.Map<UserDTO>(user);
                userDTO.Role = await GetUserRole(user);

                AddRefreshTokenResponseProperties(response, model.DeviceId, userRefreshToken?.ExpiresAt, userDTO);

                await SaveRefreshToken(response.RefreshToken, response.RefreshTokenExpiry.Value, user, model.DeviceId, userRefreshToken.UserAgent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while creating the Refresh token.");
            }
            
            return response;
        }

        private void AddRefreshTokenResponseProperties(LoginResponse response, string deviceId, DateTime? refreshTokenExpiry, UserDTO userDTO)
        {
            response.AccessToken = GenerateAccessToken(userDTO);
            response.RefreshToken = GenerateRefreshToken();
            response.RefreshTokenExpiry = refreshTokenExpiry;
            response.DeviceId = deviceId;
            response.IsLoggedIn = true;
            response.User = userDTO;
        }

        public async Task<UserDTO> GetUserFromAccessToken(string accessToken)
        {
            try
            {
                if (!string.IsNullOrEmpty(accessToken))
                {
                    var principal = GetTokenPrincipal(accessToken);
                    if (principal?.Identity?.Name is not null)
                    {
                        string userEmail = principal?.Identity?.Name;
                        var user = await _userManager.FindByEmailAsync(userEmail);
                        var userDTO = _mapper.Map<UserDTO>(user);
                        userDTO.Role = await GetUserRole(user);
                        userDTO.IsAdmin = userDTO.Role == "Admin";
                        return userDTO;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving the user from Access Token");
            }
            
            return null;
        }
        
    }
}
