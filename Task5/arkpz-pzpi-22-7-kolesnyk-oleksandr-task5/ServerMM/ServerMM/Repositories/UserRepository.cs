using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using ServerMM.Dtos;
using ServerMM.Interfaces;
using ServerMM.Models;
using System.Net;
using System.Net.Mail;
using System.Reflection;

namespace ServerMM.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqliteDBContext context;

        public UserRepository(SqliteDBContext context)
        {
            this.context = context;
        }

        private async Task<bool> SendEmailAsync(string recipientEmail, string subject, string body)
        {
            try
            {
                MailAddress from = new MailAddress("oleksandr.kolesnyk@nure.ua");
                MailAddress to = new MailAddress(recipientEmail);
                MailMessage m = new MailMessage(from, to);
                m.Subject = subject;
                m.IsBodyHtml = false;
                m.Body = body;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("oleksandr.kolesnyk@nure.ua", "qwert2004"),
                    EnableSsl = true
                };
                smtp.Send(m);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка з поштою:" + ex.Message);
                return false;
            }
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool CheckPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public async Task<IdentityResult> Register(RegisterDto registerDto)
        {
            if (registerDto.Gender != "Male" && registerDto.Gender != "Female")
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Оберіть справжній гендер. Так я вважаю шо є тільки 2 гендера. Закенселіть мене, якщо захочете"
                });
            }

            string hashedPassword = HashPassword(registerDto.Password);



            User user = new User
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                DateOfBirth = registerDto.DateOfBirth,
                Gender = registerDto.Gender,
                PasswordHash = hashedPassword,
                EmergencyEmail = registerDto.EmergencyEmail
            };

            await context.Users.AddAsync(user);

            await context.SaveChangesAsync();

            Models.UserOptions userOptions = new Models.UserOptions
            {
                UserId = user.UserId,
                MinPulse = 60,
                MaxPulse = 100,
                MinOxygenLevel = 95,
                MinBodyTemperature = 36.5,
                MaxBodyTemperature = 37.5
            };

            // Добавляем настройки пользователя
            await context.UserOptions.AddAsync(userOptions);
            int changes = await context.SaveChangesAsync();

            if (changes > 0)
            {
                return IdentityResult.Success;
            }
            else
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Помилка при регістрації"
                });
            }

        }

        public async Task<IdentityResult> Login(LoginDto loginDto, string ip)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Такого аккаунта не існує" });
            }

            if (user.isBanned)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Користувача заблоковано" });
            }

            if (CheckPassword(loginDto.Password, user.PasswordHash))
            {
                UserLogin login = new UserLogin() { UserId = user.UserId, IPAddress = ip };
                await context.UserLogins.AddAsync(login);
                await context.SaveChangesAsync();
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError { Description = "Неправильний пароль чи пошта" });
        }

        public async Task<IdentityResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.Email == forgotPasswordDto.Email);

            if (user.FirstName != forgotPasswordDto.FirstName || user.DateOfBirth != forgotPasswordDto.DateOfBirth)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Некоректні дані" });
            }

            Random random = new Random();

            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            int length = random.Next(10, 20);

            string newPassword = new string(Enumerable.Range(0, length)
            .Select(_ => validChars[random.Next(validChars.Length)])
            .ToArray()); ;

            string hashedNewPassword = HashPassword(newPassword);

            user.PasswordHash = hashedNewPassword;
            context.Users.Update(user);
            await context.SaveChangesAsync();

            if (await SendEmailAsync(forgotPasswordDto.Email,
                "Скидання пароля", "Ваш новий пароль на MedMon: " + newPassword))
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError { Description = "Шось пішло не по плану" });
        }



        public async Task<List<User>> GetUsers()
        {
            var users = await context.Users.ToListAsync();

            foreach (var item in users)
            {
                item.userOptions = await context.UserOptions.FirstOrDefaultAsync(u => u.UserId == item.UserId);
            }

            return users;
        }

        public async Task<IdentityResult> UpdateProfile(int userId, UpdateProfileDto updateProfileDto)
        {
            if (updateProfileDto.Gender != "Male" && updateProfileDto.Gender != "Female")
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Оберіть справжній гендер. Так я вважаю шо є тільки 2 гендера. Закенселіть мене, якщо захочете"
                });
            }

            var user = await context.Users.FindAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError() { Description = "Профіль не був знайдений" });

            if(user.FirstName == "Admin")
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Неможливо змінити профіль адміністратора"
                });
            }

            user.FirstName = updateProfileDto.FirstName;
            user.LastName = updateProfileDto.LastName;
            user.DateOfBirth = updateProfileDto.DateOfBirth;
            user.Gender = updateProfileDto.Gender;

            context.Update(user);
            await context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateUserOptions(int userId, UpdateUserOptionsDto updateUserOptionsDto)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError() { Description = "Профіль не був знайдений" });


            if (user.FirstName == "Admin")
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Неможливо змінити профіль адміністратора"
                });
            }


            var userOptions = await context.UserOptions.FindAsync(userId);
            if (userOptions == null)
                return IdentityResult.Failed(new IdentityError() { Description = "Налаштування для користувача не знайдені" });

            var validationResult = ValidateUserOptions(updateUserOptionsDto);
            if (!validationResult.Succeeded)
                return validationResult;

            userOptions.MinPulse = updateUserOptionsDto.MinPulse;
            userOptions.MaxPulse = updateUserOptionsDto.MaxPulse;
            userOptions.MinOxygenLevel = updateUserOptionsDto.MinOxygenLevel;
            userOptions.MinBodyTemperature = updateUserOptionsDto.MinBodyTemperature;
            userOptions.MaxBodyTemperature = updateUserOptionsDto.MaxBodyTemperature;

            context.Update(userOptions);
            await context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        private IdentityResult ValidateUserOptions(UpdateUserOptionsDto dto)
        {
            var errors = new List<IdentityError>();

            if (dto.MinPulse < 30 || dto.MaxPulse > 200 || dto.MinPulse >= dto.MaxPulse)
                errors.Add(new IdentityError
                {
                    Description = "Пульс має бути у межах від  30 до 200 ударів за хвилину, мінімальне значення повинно бути меншим за максимальне."
                });

            if (dto.MinOxygenLevel < 90)
                errors.Add(new IdentityError
                {
                    Description = "Рівень кисню має бути у межах від 90% до 100%."
                });

            if (dto.MinBodyTemperature < 30.0 || dto.MaxBodyTemperature > 45.0 || dto.MinBodyTemperature >= dto.MaxBodyTemperature)
                errors.Add(new IdentityError
                {
                    Description = "Температура тіла має бути у межах від 30°C до 45°C, мінімальне значення повинно бути меншим за максимальне."
                });

            return errors.Count == 0
                ? IdentityResult.Success
                : IdentityResult.Failed(errors.ToArray());
        }

        public async Task<IdentityResult> BanUser(int userId)
        {
            var user = await context.Users.FindAsync(userId);

            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            user.isBanned = true;
            context.Update(user);
            await context.SaveChangesAsync();

            return IdentityResult.Success;
        }
        public async Task<IdentityResult> UnBanUser(int userId)
        {
            var user = await context.Users.FindAsync(userId);

            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            user.isBanned = false;
            context.Update(user);
            await context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<bool> IfAdmin(string password)
        {
            if (password == null)
            {
                return false;
            }
            string hash = await context.Users.Where(u => u.FirstName == "Admin").Select(u => u.PasswordHash).FirstOrDefaultAsync();

            if(CheckPassword(password, hash))
            {
                return true;
            }

            return false;
        }
    }
}
