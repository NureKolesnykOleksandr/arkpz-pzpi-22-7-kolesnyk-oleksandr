using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerMM.Models
{
    public class Recommendation
    {
        [Key]
        public int RecommendationID { get; set; }

        public int UserID { get; set; }

        public User User { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.Now;

        public string RecommendationText { get; set; }
    }
}
