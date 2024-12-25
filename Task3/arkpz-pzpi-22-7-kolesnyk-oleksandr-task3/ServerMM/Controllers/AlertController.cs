using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerMM.Dtos;
using ServerMM.Interfaces;
using ServerMM.Repositories;

namespace ServerMM.Controllers
{
    [Route("api/alert")]
    [ApiController]
    public class AlertController : Controller
    {
        private readonly IAlertRepository alertRepository;

        public AlertController(IAlertRepository alertRepository)
        {
            this.alertRepository = alertRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlert([FromBody] CreateAlertDto createAlertDto)
        {
            var result = await alertRepository.CreateAlert(createAlertDto);
            if(result==IdentityResult.Success)
            {
                return Ok("Повідомлення відправлено успішно");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}
