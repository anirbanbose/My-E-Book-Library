using MyEbookLibrary.Common.DTO.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.AutheticationDTO
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public UserDTO User { get; set; }
        public bool IsLoggedIn { get; set; } = false;
        public string DeviceId { get; set; }
    } 
}
