namespace GerenciadorDeProjetos.Application.DTOs
{
    public record LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
