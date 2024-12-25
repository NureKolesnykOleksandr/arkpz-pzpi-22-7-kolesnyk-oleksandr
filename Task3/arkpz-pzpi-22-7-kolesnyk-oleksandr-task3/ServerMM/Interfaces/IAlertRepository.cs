using Microsoft.AspNetCore.Identity;
using ServerMM.Dtos;

namespace ServerMM.Interfaces
{
    public interface IAlertRepository
    {
        Task<IdentityResult> CreateAlert(CreateAlertDto alertDto);
    }
}
