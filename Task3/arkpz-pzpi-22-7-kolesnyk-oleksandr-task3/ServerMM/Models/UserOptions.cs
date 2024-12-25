using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServerMM.Models
{
    public record UserOptions
    {
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public int MinPulse { get; set; }
        public int MaxPulse { get; set; }
        public int MinOxygenLevel { get; set; }
        public double MinBodyTemperature { get; set; }
        public double MaxBodyTemperature { get; set; }
        
        [JsonIgnore]
        public User User { get; set; }
    }
}
