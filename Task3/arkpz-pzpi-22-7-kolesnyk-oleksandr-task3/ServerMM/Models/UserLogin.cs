using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerMM.Models
{
    public class UserLogin
    {
        [Key]
        public int LoginId { get; set; }

        public int UserId { get; set; }
        public DateTime LoginTime { get; set; } = DateTime.Now;

        [MaxLength(45)]
        public string IPAddress { get; set; }
    }
}
