using Backend.Api.DTOs.AuthDTOs;

namespace Backend.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    }
}