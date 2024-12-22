using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerMM.Models
{
    public class Recommendation
    {
        [Key]
        public int RecommendationID { get; set; }

        [Required]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.Now;

        [Required]
        public string RecommendationText { get; set; }
    }
}
