using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerMM.Dtos;
using ServerMM.Interfaces;
using ServerMM.Repositories;

namespace ServerMM.Controllers
{
    [Route("api/recommendation")]
    [ApiController]
    public class RecommendationController : Controller
    {
        private readonly IRecomendationRepository recomendationRepository;

        public RecommendationController(IRecomendationRepository recomendationRepository)
        {
            this.recomendationRepository = recomendationRepository;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllRecommendationsForUser(int userId)
        {
            try
            {
                var recommendations = await recomendationRepository.GetAllRecommendationsForUser(userId);

                if (recommendations == null || !recommendations.Any())
                {
                    return NotFound("Не були знайдені рекомендації для цього User");
                }

                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("{userId}/generate")]
        public async Task<IActionResult> GenerateRecommendationForUser(int userId)
        {
            try
            {
                var recommendation = await recomendationRepository.GenerateRecommendationForUser(userId);

                if (recommendation == null)
                {
                    return BadRequest("Failed to generate recommendation.");
                }

                return Ok(recommendation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
