using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerMM.Dtos;
using ServerMM.Interfaces;
using ServerMM.Models;

namespace ServerMM.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly SqliteDBContext context;

        public DeviceRepository(SqliteDBContext context)
        {
            this.context = context;
        }

        public async Task<IdentityResult> AddDevice(CreateDeviceDto addDeviceDto)
        {
            var device = new Device
            {
                DeviceName = addDeviceDto.DeviceName,
                DeviceType = addDeviceDto.DeviceType,
                SerialNumber = addDeviceDto.SerialNumber,
                UserId = addDeviceDto.UserId
            };

            context.Devices.Add(device);
            await context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> AddSensorData(string SerialNumber, AddSensorDataDto addSensorDataDto)
        {
            Device device = await context.Devices.FirstOrDefaultAsync(d => d.SerialNumber == SerialNumber);

            if(device == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Не існує девайсу з таким серійним номером" });
            }

            var sensorData = new SensorData
            {
                DeviceId = device.DeviceId,
                Timestamp = DateTime.Now,
                HeartRate = addSensorDataDto.HeartRate,
                BloodOxygenLevel = addSensorDataDto.BloodOxygenLevel,
                BodyTemperature = addSensorDataDto.BodyTemperature,
                ActivityLevel = addSensorDataDto.ActivityLevel,
                SleepPhase = addSensorDataDto.SleepPhase
            };

            context.SensorData.Add(sensorData);
            await context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteDevice(int deviceId)
        {
            var device = await context.Devices.FindAsync(deviceId);

            if (device == null)
                return IdentityResult.Failed(new IdentityError { Description = "Device not found." });

            context.Devices.Remove(device);
            await context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteDeviceHistory(int deviceId)
        {
            var sensorDataList = await context.SensorData
                .Where(sd => sd.DeviceId == deviceId)
                .ToListAsync();

            if (!sensorDataList.Any())
                return IdentityResult.Failed(new IdentityError { Description = "No sensor data found for this device." });

            context.SensorData.RemoveRange(sensorDataList);
            await context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<List<SensorData>> GetDeviceHistory(int deviceId)
        {
            return await context.SensorData
                .Where(sd => sd.DeviceId == deviceId)
                .OrderByDescending(sd => sd.Timestamp)
                .ToListAsync();
        }

        public async Task<List<Device>> GetDevices(int userId)
        {
            return await context.Devices
                .Where(d => d.UserId == userId)
                .ToListAsync();
        }

        public async Task<IdentityResult> UpdateDevice(int deviceId, UpdateDeviceDto updateDeviceDto)
        {
            var device = await context.Devices.FindAsync(deviceId);

            if (device == null)
                return IdentityResult.Failed(new IdentityError { Description = "Device not found." });

            device.DeviceName = updateDeviceDto.DeviceName;
            device.DeviceType = updateDeviceDto.DeviceType;

            context.Devices.Update(device);
            await context.SaveChangesAsync();

            return IdentityResult.Success;
        }
    }
}
