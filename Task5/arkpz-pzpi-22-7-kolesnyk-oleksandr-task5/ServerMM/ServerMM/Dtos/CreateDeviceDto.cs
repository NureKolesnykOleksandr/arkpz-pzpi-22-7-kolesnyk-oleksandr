using System.ComponentModel.DataAnnotations;

namespace ServerMM.Dtos
{
    public record CreateDeviceDto
    {
        [Required]
        public string DeviceName { get; init; }

        [Required]
        public string DeviceType { get; init; }

        [Required]
        public string SerialNumber { get; init; }

        [Required]
        public int UserId { get; init; }

    }
}