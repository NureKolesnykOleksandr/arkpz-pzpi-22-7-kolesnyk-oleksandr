using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ServerMM.Dtos;
using ServerMM.Models;

namespace ServerMM.Interfaces
{
    public interface IDeviceRepository
    {
        Task<IdentityResult> AddDevice(CreateDeviceDto addDeviceDto);

        Task<IdentityResult> AddSensorData(string SerialNumber, AddSensorDataDto addSensorDataDto);

        Task<List<Device>> GetDevices(int userId);

        Task<IdentityResult> UpdateDevice(int deviceId, UpdateDeviceDto updateDeviceDto);

        Task<IdentityResult> DeleteDevice(int deviceId);

        Task<List<SensorData>> GetDeviceHistory(int deviceId);
        Task<IdentityResult> DeleteDeviceHistory(int deviceId);
    }
}
