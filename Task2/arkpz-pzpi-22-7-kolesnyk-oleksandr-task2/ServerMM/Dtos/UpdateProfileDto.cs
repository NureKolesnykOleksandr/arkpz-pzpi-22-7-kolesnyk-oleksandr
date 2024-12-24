using System.ComponentModel.DataAnnotations;

namespace ServerMM.Dtos
{
    public record UpdateProfileDto
    {
        [Required]
        public string? FirstName { get; init; }

        [Required]
        public string? LastName { get; init; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; init; }

        [Required]
        public string? Gender { get; init; } // Enum values: Male, Female, Other
    }
}