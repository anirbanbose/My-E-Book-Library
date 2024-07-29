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
    [Table("Role", Schema = "EbookLibrary")]
    public partial class Role : IdentityRole<int>
    {
        public Role()
        {            
        }
        [SetsRequiredMembers]
        public Role(string name)
        {
            Name = name;
        }
        [MaxLength(100)]
        [NotNull()]
        public override required string Name { get; set; }
        public DateTime AddUpdateDate { get; set; }

        public virtual ICollection<IdentityRoleClaim<int>> Claims { get; set; } = new HashSet<IdentityRoleClaim<int>>();
        public virtual ICollection<IdentityUserRole<int>> UserRoles { get; set; } = new HashSet<IdentityUserRole<int>>();
    }
}
