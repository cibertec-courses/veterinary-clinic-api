
using VeterinaryClinic.Application.DTOs.Auth;

namespace VeterinaryClinic.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> ResgiterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    }
    
}