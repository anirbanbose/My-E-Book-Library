using MyEbookLibrary.Data.EF.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Data.EF.Entities
{
    [Table("UserRefreshToken", Schema = "EbookLibrary")]
    public partial class UserRefreshToken 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string RefreshToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string DeviceUniqueId { get; set; }
        public string UserAgent { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
