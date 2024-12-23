using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerMM.Models
{
    public class Device
    {
        [Key]
        public int DeviceID { get; set; }

        [MaxLength(100)]
        public string DeviceName { get; set; }

        public int UserID { get; set; }

        public User User { get; set; }

        [MaxLength(50)]
        public string DeviceType { get; set; }
           
        [MaxLength(100)]
        public string SerialNumber { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.Now;

        public ICollection<SensorData> SensorData { get; set; }
    }
}
