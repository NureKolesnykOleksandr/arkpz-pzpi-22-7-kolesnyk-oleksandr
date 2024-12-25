using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerMM.Models
{
    public class Device
    {
        [Key]
        public int DeviceId { get; set; }

        [MaxLength(100)]
        public string DeviceName { get; set; }

        public int UserId { get; set; }

        [MaxLength(50)]
        public string DeviceType { get; set; }
           
        [MaxLength(100)]
        public string SerialNumber { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.Now;
    }
}
