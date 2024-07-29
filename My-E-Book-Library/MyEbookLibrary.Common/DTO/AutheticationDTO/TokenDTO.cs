using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.AutheticationDTO
{
    public class TokenDTO
    {
        public string Email { get; set; }

        public string? AccessToken { get; set; } = null;
        public string? RefreshToken { get; set; } = null;
        public string DeviceId { get; set; }
    }
}
