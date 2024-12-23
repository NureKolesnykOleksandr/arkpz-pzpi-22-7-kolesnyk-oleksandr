using System.ComponentModel.DataAnnotations;

namespace ServerMM.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(10)]
        public string Gender { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Device> Devices { get; set; }
        public ICollection<Alert> Alerts { get; set; }
        public ICollection<Recommendation> Recommendations { get; set; }
        public ICollection<UserLogin> UserLogins { get; set; }
    }
}
