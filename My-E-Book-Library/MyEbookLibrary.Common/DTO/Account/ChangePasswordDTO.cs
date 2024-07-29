using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Account
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "Old Password is required")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "New Password is required")]
        [MaxLength(20, ErrorMessage = "Password can't be more than 20 characters.")]
        [MinLength(8, ErrorMessage = "Password can't be less than 8 characters.")]
        public string NewPassword { get; set; }
    }
}
