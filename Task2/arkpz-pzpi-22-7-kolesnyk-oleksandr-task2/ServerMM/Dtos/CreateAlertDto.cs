using System.ComponentModel.DataAnnotations;

namespace ServerMM.Dtos
{
    public record CreateAlertDto
    {
        [Required]
        public string AlertType { get; init; }

        [Required]
        public string AlertMessage { get; init; }

        [Required]
        public int UserId { get; init; }
    }
}
