﻿using System.ComponentModel.DataAnnotations;

namespace ServerMM.Dtos
{
    public record UpdateDeviceDto
    {
        [Required]
        public int DeviceId { get; init; }
        [Required]
        public string? DeviceName { get; init; }
        [Required]
        public string? DeviceType { get; init; }
    }
}
