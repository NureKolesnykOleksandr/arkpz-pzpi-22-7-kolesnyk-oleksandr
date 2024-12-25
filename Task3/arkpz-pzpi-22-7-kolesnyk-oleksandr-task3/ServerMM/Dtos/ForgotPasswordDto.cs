using System.ComponentModel.DataAnnotations;

namespace ServerMM.Dtos
{
    public record ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        public string FirstName { get; init; }
         
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; init; }
    }
}
