using GerenciadorDeProjetos.Domain.DTOs;
using GerenciadorDeProjetos.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorDeProjetos.Presentation.Controllers
{

    public class AuthenticationController : ControllerBase 
    {
        private readonly TokenService _tokenService;

        public AuthenticationController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var token = _tokenService.GenerateToken(loginDto);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            Response.Headers.Add("Authorization", $"{token}");

            return Ok(new { message = "Login feito com Sucesso" });
        }
    }
}
