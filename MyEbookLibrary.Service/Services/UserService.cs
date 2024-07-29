using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Author;
using MyEbookLibrary.Common.DTO.Responses;
using MyEbookLibrary.Data.EF.Entities;
using MyEbookLibrary.Data.Repository.Contracts;
using MyEbookLibrary.Email.Emails;
using MyEbookLibrary.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IAccountEmails _accountEmails;
        private readonly ILogger<UserService> _logger;
        public UserService(UserManager<User> userManager, IMapper mapper, IAccountEmails accountEmails, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _accountEmails = accountEmails;
            _logger = logger;
        }
        public async Task<DetailResult<UserDTO>> GetProfile(int userId)
        {
            try
            {
                if (userId > 0)
                {
                    var user = await _userManager.Users.FirstOrDefaultAsync(d => d.Id == userId);
                    if (user != null)
                    {
                        var userDto = _mapper.Map<UserDTO>(user);
                        return DetailResult<UserDTO>.Success(userDto);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving the user profile for user id - {0}", userId);
            }
            
            return DetailResult<UserDTO>.Failure(Error.RecordNotFound()); ;
        }

        public async Task<SaveResult<UserDTO>> SaveProfile(SaveProfileDTO user, int userId)
        {
            try
            {
                if (user != null && user.Id == userId)
                {
                    var entity = await _userManager.Users.FirstOrDefaultAsync(d => d.Id == userId);
                    if (entity != null)
                    {
                        entity.FirstName = user.FirstName;
                        entity.MiddleName = user.MiddleName;
                        entity.LastName = user.LastName;
                        entity.BirthDate = !string.IsNullOrEmpty(user.BirthDate) ? DateTime.ParseExact(user.BirthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) : null;
                        var updated = await _userManager.UpdateAsync(entity);
                        if (updated.Succeeded)
                        {
                            var userDTO = _mapper.Map<UserDTO>(entity);
                            userDTO.Role = await GetUserRole(entity);
                            await Task.Run(() => _accountEmails.SendProfileUpdateEmail(entity.Email));
                            return SaveResult<UserDTO>.Success(userDTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while saving the user profile for user id - {0}", userId);
            }
            return SaveResult<UserDTO>.Failure(Error.SaveFailure("Profile couldn't be saved."));
        }

        public async Task<SaveResult> ChangePassword(ChangePasswordDTO passwordDto, int userId)
        {
            try
            {
                if (userId != 0)
                {
                    var user = await _userManager.Users.FirstOrDefaultAsync(d => d.Id == userId);
                    if (user != null)
                    {
                        var oldPasswordMatch = await _userManager.CheckPasswordAsync(user, passwordDto.OldPassword);
                        if (oldPasswordMatch)
                        {
                            var result = await _userManager.ChangePasswordAsync(user, passwordDto.OldPassword, passwordDto.NewPassword);
                            if (result.Succeeded)
                            {
                                return SaveResult.Success();
                            }
                        }
                        else
                        {
                            return SaveResult.Failure(Error.ValidationError("Wrong Current password!"));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while changing the password for user id - {0}", userId);
            }
            return SaveResult.Failure(Error.SaveFailure("Password couldn't be changed. Please try again later."));
        }

        public async Task<bool> IsEmailAvailable(string email)
        {
            try
            {
                if(await _userManager.FindByEmailAsync(email) == null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while checking whether the email is available - {0}", email);
            }
            return false;
        }

        public async Task<SaveResult> Register(RegistrationDTO registration)
        {
            try
            {                
                var user = await _userManager.FindByEmailAsync(registration.Email.Trim());
                if (user == null)
                {
                    PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                    var email = registration.Email.Trim();
                    user = new User()
                    {
                        Email = email,
                        UserName = email,
                        FirstName = registration.FirstName,
                        LastName = registration.LastName,
                        MiddleName = registration.MiddleName,
                        NormalizedEmail = email,
                        EmailConfirmed = true
                    };
                    user.PasswordHash = passwordHasher.HashPassword(user, registration.Password);
                    var result = await _userManager.CreateAsync(user);

                    if (result.Succeeded)
                    {
                        var roleResult = await _userManager.AddToRoleAsync(user, "Member");
                        if(roleResult.Succeeded)
                        {
                            Task.Run(() => _accountEmails.SendRegistrationEmail(registration.Email));
                            return SaveResult.Success();
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while registering the user - {0}", registration.Email);
            }
            return SaveResult.Failure(Error.SaveFailure("Registration couldn't be done! Please try again later."));
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving the role for the user - {0}", user.Email);
            }
            
            return string.Empty;
        }

    }
}
