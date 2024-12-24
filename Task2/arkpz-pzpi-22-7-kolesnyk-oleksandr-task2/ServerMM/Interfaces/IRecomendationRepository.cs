using ServerMM.Models;

namespace ServerMM.Interfaces
{
    public interface IRecomendationRepository
    {
        Task<IEnumerable<Recommendation>> GetAllRecommendationsForUser(int userId);
        Task<Recommendation> GenerateRecommendationForUser(int userId);
    }
}
