using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GerenciadorDeProjetos.Domain.DTOs;
using GerenciadorDeProjetos.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace GerenciadorDeProjetos.Infrastructure.Interfaces
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        private readonly UsuarioRepository _usuarioRepository;

        public TokenService(IConfiguration configuration, UsuarioRepository usuarioRepository)
        {
            _configuration = configuration;
            _usuarioRepository = usuarioRepository;

        }
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public string GenerateToken(LoginDto user)
        {
            var userDataBase = _usuarioRepository.GetByUsername(user.Username);
            if (userDataBase == null || !BCrypt.Net.BCrypt.Verify(user.Password, userDataBase.Password))
            {
                return string.Empty;
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"] ?? string.Empty));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                claims: new[]
                {
            new Claim(type: ClaimTypes.Name, user.Username),
            new Claim("Id", userDataBase.Id.ToString())
                },
                expires: DateTime.Now.AddHours(12),
                signingCredentials: signinCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;
        }

        public int GetIdToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                var userId = jsonToken?.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                Console.WriteLine($"ID extraído: {userId}");

                if (int.TryParse(userId, out int id))
                {
                    return id;
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao extrair ID do token: {ex.Message}");
                return 0;
            }
        }


    }
}
