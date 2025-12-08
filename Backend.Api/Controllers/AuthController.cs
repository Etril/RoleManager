using Backend.Api.DTOs;
using Backend.Api.DTOs.AuthDTOs;
using Backend.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController (IAuthService authService)
    {
       _authService= authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto dto)
    {
        var result= await _authService.LoginAsync(dto);
        return Ok(result);
    }
}