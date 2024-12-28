using System.ComponentModel.DataAnnotations;

namespace ServerMM.Dtos
{
    public record UpdateUserOptionsDto
    {
        [Required]
        public int MinPulse { get; set; }
        [Required]
        public int MaxPulse { get; set; }
        [Required]
        public int MinOxygenLevel { get; set; }
        [Required]
        public double MinBodyTemperature { get; set; }
        [Required]
        public double MaxBodyTemperature { get; set; }
    }
}
