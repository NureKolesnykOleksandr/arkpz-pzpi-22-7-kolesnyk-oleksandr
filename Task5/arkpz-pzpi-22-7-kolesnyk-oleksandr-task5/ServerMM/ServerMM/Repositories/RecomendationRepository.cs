using Microsoft.EntityFrameworkCore;
using ServerMM.Interfaces;
using ServerMM.Models;
using System.Linq;

namespace ServerMM.Repositories
{
    public class RecomendationRepository : IRecomendationRepository
    {
        private readonly SqliteDBContext context;

        // Конструктор класу для ініціалізації контексту бази даних
        public RecomendationRepository(SqliteDBContext context)
        {
            this.context = context;
        }

        // Метод для отримання всіх рекомендацій для конкретного користувача
        public async Task<IEnumerable<Recommendation>> GetAllRecommendationsForUser(int userId)
        {
            return await context.Recommendations
                .Where(r => r.UserId == userId) // Фільтрація рекомендацій за userId
                .ToListAsync(); // Асинхронне отримання списку рекомендацій
        }

        // Метод для генерації рекомендацій для користувача на основі його даних
        public async Task<Recommendation> GenerateRecommendationForUser(int userId)
        {
            // Перевірка, чи існує користувач із заданим userId
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new ArgumentException($"Користувача з ID {userId} не знайдено.");
            }

            // Отримання останніх даних із сенсорів для користувача
            var lastSensorData = await context.SensorData
                .Where(s => s.Device.UserId == userId)
                .OrderByDescending(s => s.Timestamp) // Сортування за часом у порядку спадання
                .FirstOrDefaultAsync(); // Отримання останнього запису

            if (lastSensorData == null)
            {
                throw new InvalidOperationException("Немає даних датчиків для цього користувача.");
            }

            // Отримання налаштувань користувача
            var userOptions = await context.UserOptions.FirstOrDefaultAsync(u => u.UserId == userId);

            if (userOptions == null)
            {
                throw new InvalidOperationException("Налаштування користувача не задано.");
            }

            // Ініціалізація змінних для формування рекомендації
            bool isfine = true; // Чи в нормі всі показники
            string recommendationText = ""; // Текст рекомендації
            List<string> articles = new(); // Список статей для допомоги
            bool isCritical = false; // Чи є ситуація критичною

            // Аналіз частоти пульсу
            if (lastSensorData.HeartRate < userOptions.MinPulse)
            {
                var diff = userOptions.MinPulse - lastSensorData.HeartRate;
                recommendationText = $"Пульс занадто низький на {diff} уд./хв. Рекомендується збільшити активність або звернутися до лікаря.";
                articles.Add("https://medikom.ua/chto-takoe-bradikardiya/");
                if (diff > 20) isCritical = true; // Якщо відхилення більше 20, ситуація критична
                isfine = false;
            }
            else if (lastSensorData.HeartRate > userOptions.MaxPulse)
            {
                var diff = lastSensorData.HeartRate - userOptions.MaxPulse;
                recommendationText = $"Пульс занадто високий на {diff} уд./хв. Виконайте вправи для заспокоєння.";
                articles.Add("https://dobrobut.com/ua/med/c-tahikardia-kak-ponizit-puls-v-domasnih-usloviah");
                if (diff > 20) isCritical = true; // Якщо відхилення більше 20, ситуація критична
                isfine = false;
            }

            // Аналіз температури тіла
            if (lastSensorData.BodyTemperature < userOptions.MinBodyTemperature)
            {
                var diff = userOptions.MinBodyTemperature - lastSensorData.BodyTemperature;
                recommendationText += $" Температура тіла занадто низька на {diff:0.0}°C. Зігрійтеся та зверніться до лікаря.";
                articles.Add("https://rplus.com.ua/blog/nizkaya-temperatura-tela-chto-delat/");
                if (diff > 2.0) isCritical = true; // Якщо відхилення більше 2°C, ситуація критична
                isfine = false;
            }
            else if (lastSensorData.BodyTemperature > userOptions.MaxBodyTemperature)
            {
                var diff = lastSensorData.BodyTemperature - userOptions.MaxBodyTemperature;
                recommendationText += $" Температура тіла занадто висока на {diff:0.0}°C. Зверніться до лікаря.";
                articles.Add("https://omegamc.ua/ua/spravochnik/klinicheskij/vysokaya-temperatura-chto-delat.html");
                if (diff > 2.0) isCritical = true; // Якщо відхилення більше 2°C, ситуація критична
                isfine = false;
            }

            // Аналіз рівня кисню в крові
            if (lastSensorData.BloodOxygenLevel < userOptions.MinOxygenLevel)
            {
                var diff = userOptions.MinOxygenLevel - lastSensorData.BloodOxygenLevel;
                recommendationText += $" Рівень кисню занадто низький на {diff:0.0}%. Рекомендується виконати дихальні вправи.";
                articles.Add("https://medikom.ua/gipoksiya-vidy-lechenie-kislorodnoj-nedostatochnosti/");
                isfine = false;
            }

            // Додавання тексту для критичної ситуації
            if (isCritical)
            {
                recommendationText += $" У вас критична ситуація, рекомендуємо негайно повідомити свій Emergency контакт або викликати швидку.";
            }

            // Додавання статей, якщо є порушення показників
            if (isfine == false)
            {
                recommendationText += "\nСтатті для допомоги: " + string.Join(", ", articles);
            }
            else
            {
                recommendationText = "Усі показники в нормі. Дій не потрібно.";
            }

            // Створення об'єкта рекомендації
            var recommendation = new Recommendation
            {
                UserId = userId,
                RecommendationText = recommendationText,
                GeneratedAt = DateTime.UtcNow
            };

            // Збереження рекомендації в базі даних
            context.Recommendations.Add(recommendation);
            await context.SaveChangesAsync();

            // Повернення створеної рекомендації
            return recommendation;
        }
    }
}
