using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ServerMM.Models
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(new User
            {
                UserId = 1,
                FirstName = "Admin",
                LastName = "Kolesnyk",
                Email = "oleksandr.kolesnyk@nure.ua",
                EmergencyEmail = "oleksandr.kolesnyk@nure.ua",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("xNHtw1Jm2gzxi5EKXWM1Pgs7vyMksVd0i8ovp13uMhAMy7exjU"),
                Gender = "Male",
                CreatedAt = DateTime.Now
            });
        }
    }

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

        public bool isBanned { get; set; } = false;

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(10)]
        public string Gender { get; set; }

        public UserOptions userOptions { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
