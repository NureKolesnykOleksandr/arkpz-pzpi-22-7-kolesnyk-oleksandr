using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerMM.Models
{
    public class Alert
    {
        [Key]
        public int AlertId { get; set; }
       
        public int UserID { get; set; }

        public User User { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string AlertType { get; set; }

        public string AlertMessage { get; set; }

        public bool IsAcknowledged { get; set; } = false;
    }
}
