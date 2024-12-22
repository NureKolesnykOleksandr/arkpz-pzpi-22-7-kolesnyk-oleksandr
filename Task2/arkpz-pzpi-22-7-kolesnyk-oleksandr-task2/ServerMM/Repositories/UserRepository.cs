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
        private readonly SqliteDBContext _dbContext;

        public UserRepository(SqliteDBContext dbContext)
        {
            _dbContext = dbContext;
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
            catch(Exception ex) 
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
            if(registerDto.Gender!="Male" || registerDto.Gender != "Female")
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
                PasswordHash = hashedPassword
            };

            await _dbContext.Users.AddAsync(user);

            int changes = await _dbContext.SaveChangesAsync();

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

        public async Task<IdentityResult> Login(LoginDto loginDto)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Такого аккаунта не існує" });
            }

            if (CheckPassword(loginDto.Password, user.PasswordHash))
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError { Description = "Неправильний пароль чи пошта" });
        }

        public async Task<IdentityResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == forgotPasswordDto.Email);

            if(user.FirstName!=forgotPasswordDto.FirstName || user.DateOfBirth != forgotPasswordDto.DateOfBirth)
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
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            if (await SendEmailAsync(forgotPasswordDto.Email,
                "Скидання пароля","Ваш новий пароль на MedMon: "+ newPassword))
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError { Description = "Шось пішло не по плану" });
        }
    }
}
