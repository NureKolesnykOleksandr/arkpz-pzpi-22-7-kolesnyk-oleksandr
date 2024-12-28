using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerMM.Dtos;
using ServerMM.Interfaces;
using ServerMM.Repositories;

namespace ServerMM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : Controller
    {
        private readonly IDeviceRepository deviceRepository;

        public DeviceController(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddDevice([FromBody] CreateDeviceDto addDeviceDto)
        {
            var result = await deviceRepository.AddDevice(addDeviceDto);
            if (result == IdentityResult.Success)
            {
                return Ok("Датчик додано успішно");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("SensorData/{SerialNumber}")]
        public async Task<IActionResult> AddSensorReading(string SerialNumber, [FromBody] AddSensorDataDto addSensorDataDto)
        {
            var result = await deviceRepository.AddSensorData(SerialNumber, addSensorDataDto);
            if (result == IdentityResult.Success)
            {
                return Ok("Датчик успішно оновлено");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetDevices(int userId)
        {
            var devices = await deviceRepository.GetDevices(userId);
            return Ok(devices);
        }

        [HttpPut("{deviceId}")]
        public async Task<IActionResult> UpdateDevice(int deviceId, [FromBody] UpdateDeviceDto updateDeviceDto)
        {
            var result = await deviceRepository.UpdateDevice(deviceId, updateDeviceDto);
            if (result == IdentityResult.Success)
            {
                return Ok("Датчик успішно оновлено");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpDelete("{deviceId}/history")]
        public async Task<IActionResult> DeleteDeviceHiatory(int deviceId)
        {
            var result = await deviceRepository.DeleteDeviceHistory(deviceId);
            if (result == IdentityResult.Success)
            {
                return Ok("Історію датчика успішно зітрано");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet("{deviceId}/history")]
        public async Task<IActionResult> GetDeviceHistory(int deviceId)
        {
            var history = await deviceRepository.GetDeviceHistory(deviceId);
            if (history.Count>0)
            {
                return Ok(history);
            }
            else
            {
                return BadRequest("Шось пішло не так");
            }
        }


    }
}
