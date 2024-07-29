using MyEbookLibrary.Data.EF.Entities.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Data.EF.Entities
{
    [Table("User", Schema = "EbookLibrary")]
    public partial class User : IdentityUser<int>
    {
        public User()
        {            
        }

        [SetsRequiredMembers]
        public User(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;            
        }
        [MaxLength(100)]
        [NotNull()]
        public required string FirstName { get; set; }
        [MaxLength(100)]
        [NotNull()]
        public required string LastName { get; set; }
        [MaxLength(100)]
        public string? MiddleName { get; set; } = null;
        public DateTime? BirthDate { get; set; }

        [MaxLength(250)]
        [NotNull()]
        public required override string Email { get; set; }

        [NotNull()]
        public override string PasswordHash { get; set; }


        public bool Deleted { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        public virtual ICollection<IdentityUserClaim<int>> Claims { get; set; } = new HashSet<IdentityUserClaim<int>>();
        public virtual ICollection<IdentityUserLogin<int>> Logins { get; set; } = new HashSet<IdentityUserLogin<int>>();
        public virtual ICollection<IdentityUserToken<int>> Tokens { get; set; } = new HashSet<IdentityUserToken<int>>();
        public virtual ICollection<IdentityUserRole<int>> UserRoles { get; set; } = new HashSet<IdentityUserRole<int>>();
    }
}
