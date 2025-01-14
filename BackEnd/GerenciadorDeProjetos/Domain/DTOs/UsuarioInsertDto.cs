namespace GerenciadorDeProjetos.Domain.DTOs
{
    public class UsuarioInsertDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cargo { get; set; }
        public List<TarefaDto> Tarefas { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

    }
}
