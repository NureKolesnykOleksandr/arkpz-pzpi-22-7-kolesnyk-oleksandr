using System.ComponentModel.DataAnnotations;

namespace ServerMM.Dtos
{
    public record RegisterDto
    {
        [Required]
        public string FirstName { get; init; }

        [Required]
        public string LastName { get; init; }

        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        [EmailAddress]
        public string EmergencyEmail { get; init; }

        [Required]
        [MinLength(6)]
        public string Password { get; init; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; init; }

        [Required]
        public string Gender { get; init; } // Enum values: Male, Female, Other
    }
}
