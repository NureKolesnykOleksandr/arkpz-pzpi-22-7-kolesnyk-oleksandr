using System.ComponentModel.DataAnnotations;

namespace ServerMM.Models
{

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [EmailAddress]
        public string EmergencyEmail { get; set; }

        public string PasswordHash { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(10)]
        public string Gender { get; set; }

        public UserOptions userOptions { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
