using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerMM.Dtos;
using ServerMM.Interfaces;
using ServerMM.Repositories;

namespace ServerMM.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IUserRepository userRepository;

        public AdminController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpDelete("ban/{userId}")]
        public async Task<IActionResult> BanUser(int userId, string password)
        {
            if (await userRepository.IfAdmin(password))
            {
                userRepository.BanUser(userId);
            }
            else
            {
                return Unauthorized();
            }
            return Ok("Користувача успішно забанено");

        }

        [HttpPut("unban/{userId}")]
        public async Task<IActionResult> UnbanUser(int userId, string password)
        {
            if (await userRepository.IfAdmin(password))
            {
                userRepository.UnBanUser(userId);
            }
            else
            {
                return Unauthorized();
            }
            return Ok("Користувача успішно розбанено");

        }
    }
}
