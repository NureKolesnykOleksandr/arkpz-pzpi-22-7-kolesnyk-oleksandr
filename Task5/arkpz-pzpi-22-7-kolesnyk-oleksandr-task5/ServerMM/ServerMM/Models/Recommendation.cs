    using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServerMM.Models
{
    public class Recommendation
    {
        [Key]
        public int RecommendationId { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.Now;

        public string RecommendationText { get; set; }
    }
}
