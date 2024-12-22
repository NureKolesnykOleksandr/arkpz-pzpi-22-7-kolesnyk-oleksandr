using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerMM.Models
{
    public class Alert
    {
        [Key]
        public int AlertID { get; set; }

        [Required]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(50)]
        public string AlertType { get; set; }

        [Required]
        public string AlertMessage { get; set; }

        public bool IsAcknowledged { get; set; } = false;
    }
}
