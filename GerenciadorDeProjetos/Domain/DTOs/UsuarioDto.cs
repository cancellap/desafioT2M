using GerenciadorDeProjetos.Domain.DTOs;
using GerenciadorDeProjetos.Domain.Entities;

public class UsuarioDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cargo { get; set; }
    public List<TarefaDto> Tarefas { get; set; }

    public UsuarioDto(Usuario usuario)
    {
        if (usuario == null)
            throw new ArgumentNullException(nameof(usuario), "O usuário não pode ser nulo.");

        Id = usuario.Id;
        Nome = usuario.Nome;
        Cargo = usuario.Cargo;
        Tarefas = usuario.Tarefas?.Select(t => new TarefaDto(t)).ToList() ?? new List<TarefaDto>();
    }
}
