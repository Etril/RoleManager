using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Backend.Api.Services.Interfaces;
using Backend.Api.DTOs.AuthDTOs;
using Backend.Application.Repositories;
using Backend.Domain.ValueObjects;

namespace Backend.Api.Services.Auth
{

    public class AuthService : IAuthService {
    private readonly IUserRepository _userRepository; 
    private readonly IConfiguration _config; 
    

    public AuthService (IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var usernameVo= new UserName(dto.Username);
            var user= await _userRepository.GetByUsernameAsync(usernameVo);

            if (user == null || !user.Password.Matches(dto.Password))
            {
                return new LoginResponseDto
                {
                    Success= false,
                    Message= "Invalid credentials"
                };
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username.Value),
                new Claim(ClaimTypes.Role, user.Role.Type.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer : _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            var jwt= new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponseDto
            {
                Success = true,
                Token = jwt
            };
        }

        
    }
    
}

