﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerMM.Models
{
    public class SensorData
    {
        [Key]
        public int DataID { get; set; }

        public int DeviceID { get; set; }

        public Device Device { get; set; }

        public int UserID { get; set; }

        public User User { get; set; }

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
