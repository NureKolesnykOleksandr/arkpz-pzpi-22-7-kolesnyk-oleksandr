using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerMM.Dtos;
using ServerMM.Interfaces;
using ServerMM.Models;
using System.Net.Mail;
using System.Net;

namespace ServerMM.Repositories
{
    public class AlertRepository : IAlertRepository
    {
        private readonly SqliteDBContext context;

        public AlertRepository(SqliteDBContext context)
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

        public async Task<IdentityResult> CreateAlert(CreateAlertDto createAlertDto)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.UserId == createAlertDto.UserId);

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = "Такого користувача не існує" });
            }

            Alert alert = new Alert() { 
            AlertMessage = createAlertDto.AlertMessage,
            AlertType = createAlertDto.AlertType,
            UserID = user.UserId
            };

            if(!(await SendEmailAsync(user.EmergencyEmail,
                $"Повідомлення від користувача: {user.FirstName} {user.LastName} на тему {createAlertDto.AlertType}", 
                createAlertDto.AlertMessage)))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Помилка при відправленні повідомлення"
                });
            }

            alert.IsAcknowledged = true;

            await context.Alerts.AddAsync(alert);
            int changes = await context.SaveChangesAsync();

            if (changes > 0)
            {
                return IdentityResult.Success;
            }
            else
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Помилка при відправленні повідомлення"
                });
            }



        }
    }
}
