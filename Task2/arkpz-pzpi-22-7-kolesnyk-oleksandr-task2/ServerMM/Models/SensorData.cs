using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerMM.Models
{
    public class SensorData
    {
        [Key]
        public int DataID { get; set; }

        [Required]
        public int DeviceID { get; set; }

        [ForeignKey("DeviceID")]
        public Device Device { get; set; }

        [Required]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        public int? HeartRate { get; set; }
        public double? BloodOxygenLevel { get; set; }
        public double? BodyTemperature { get; set; }

        [MaxLength(10)]
        public string ActivityLevel { get; set; }

        [MaxLength(10)]
        public string SleepPhase { get; set; }
    }
}
