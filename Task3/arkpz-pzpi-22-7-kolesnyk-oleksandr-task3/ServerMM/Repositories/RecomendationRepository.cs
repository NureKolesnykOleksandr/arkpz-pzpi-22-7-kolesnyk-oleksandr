using Microsoft.EntityFrameworkCore;
using ServerMM.Interfaces;
using ServerMM.Models;
using System.Linq;

namespace ServerMM.Repositories
{
    public class RecomendationRepository : IRecomendationRepository
    {
        private readonly SqliteDBContext context;

        public RecomendationRepository(SqliteDBContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Recommendation>> GetAllRecommendationsForUser(int userId)
        {
            return await context.Recommendations
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<Recommendation> GenerateRecommendationForUser(int userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new ArgumentException($"Користувача з ID {userId} не знайдено.");
            }

            var lastSensorData = await context.SensorData
                .Where(s => s.Device.UserId == userId)
                .OrderByDescending(s => s.Timestamp)
                .FirstOrDefaultAsync();

            if (lastSensorData == null)
            {
                throw new InvalidOperationException("Немає даних датчиків для цього користувача.");
            }

            var userOptions = await context.UserOptions.FirstOrDefaultAsync(u => u.UserId == userId);

            if (userOptions == null)
            {
                throw new InvalidOperationException("Налаштування користувача не задано.");
            }

            bool isfine = true;
            string recommendationText = "";
            List<string> articles = new();
            bool isCritical = false;

            if (lastSensorData.HeartRate < userOptions.MinPulse)
            {
                var diff = userOptions.MinPulse - lastSensorData.HeartRate;
                recommendationText = $"Пульс занадто низький на {diff} уд./хв. Рекомендується збільшити активність або звернутися до лікаря.";
                articles.Add("https://medikom.ua/chto-takoe-bradikardiya/");
                if (diff > 20) isCritical = true;
                isfine = false;
            }
            else if (lastSensorData.HeartRate > userOptions.MaxPulse)
            {
                var diff = lastSensorData.HeartRate - userOptions.MaxPulse;
                recommendationText = $"Пульс занадто високий на {diff} уд./хв. Виконайте вправи для заспокоєння.";
                articles.Add("https://dobrobut.com/ua/med/c-tahikardia-kak-ponizit-puls-v-domasnih-usloviah");
                if (diff > 20) isCritical = true;
                isfine = false;
            }

            if (lastSensorData.BodyTemperature < userOptions.MinBodyTemperature)
            {
                var diff = userOptions.MinBodyTemperature - lastSensorData.BodyTemperature;
                recommendationText += $" Температура тіла занадто низька на {diff:0.0}°C. Зігрійтеся та зверніться до лікаря.";
                articles.Add("https://rplus.com.ua/blog/nizkaya-temperatura-tela-chto-delat/");
                if (diff > 2.0) isCritical = true;
                isfine = false;
            }
            else if (lastSensorData.BodyTemperature > userOptions.MaxBodyTemperature)
            {
                var diff = lastSensorData.BodyTemperature - userOptions.MaxBodyTemperature;
                recommendationText += $" Температура тіла занадто висока на {diff:0.0}°C. Зверніться до лікаря.";
                articles.Add("https://omegamc.ua/ua/spravochnik/klinicheskij/vysokaya-temperatura-chto-delat.html");
                if (diff > 2.0) isCritical = true;
                isfine = false;
            }

            if (lastSensorData.BloodOxygenLevel < userOptions.MinOxygenLevel)
            {
                var diff = userOptions.MinOxygenLevel - lastSensorData.BloodOxygenLevel;
                recommendationText += $" Рівень кисню занадто низький на {diff:0.0}%. Рекомендується виконати дихальні вправи.";
                articles.Add("https://medikom.ua/gipoksiya-vidy-lechenie-kislorodnoj-nedostatochnosti/");
                isfine = false;
                }

            if (isCritical)
            {
                recommendationText += $" В вас критична ситуація, рекомендуємо негайно повідомити свій Emergency контакт, або визвати швидку";
            }


            if (isfine == false)
            {
                recommendationText += "\nСтатті для допомоги: " + string.Join(", ", articles);
            }
            else
            {
                recommendationText = "Усі показники в нормі. Дій не потрібно.";
            }

            var recommendation = new Recommendation
            {
                UserId = userId,
                RecommendationText = recommendationText,
                GeneratedAt = DateTime.UtcNow
            };

            context.Recommendations.Add(recommendation);
            await context.SaveChangesAsync();

            return recommendation;
        }
    }
}
