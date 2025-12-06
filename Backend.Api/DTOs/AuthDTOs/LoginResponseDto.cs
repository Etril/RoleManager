using System.ComponentModel.DataAnnotations;

namespace Backend.Api.DTOs.AuthDTOs
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }

        public string? Message { get; set; }

        public string? Token { get; set; }
        
    }
}