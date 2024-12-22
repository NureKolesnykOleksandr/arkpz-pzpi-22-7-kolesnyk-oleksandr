using Microsoft.AspNetCore.Identity;
using ServerMM.Dtos;

namespace ServerMM.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> Register(RegisterDto registerDto);

        Task<IdentityResult> Login(LoginDto loginDto);
        Task<IdentityResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto);
    }
}
