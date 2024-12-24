using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerMM.Dtos;
using ServerMM.Interfaces;
using ServerMM.Repositories;

namespace ServerMM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetDevices()
        {
            var users = await userRepository.GetUsers();
            return Ok(users);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateProfile(int userId, [FromBody] UpdateProfileDto updateProfileDto)
        {
            var result = await userRepository.UpdateProfile(userId,updateProfileDto);

            if (result == IdentityResult.Success)
            {
                return Ok("Профіль успішно оновлено");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPut("update-options/{userId}")]
        public async Task<IActionResult> UpdateUserOptions(int userId,[FromBody] UpdateUserOptionsDto updateUserOptionsDto)
        {
            var result = await userRepository.UpdateUserOptions(userId, updateUserOptionsDto);

            if (result == IdentityResult.Success)
            {
                return Ok("Профіль успішно оновлено");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}
