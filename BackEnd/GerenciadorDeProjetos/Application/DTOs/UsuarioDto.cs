using GerenciadorDeProjetos.Application.DTOs;
using GerenciadorDeProjetos.Domain.Entities;

public class UsuarioDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cargo { get; set; }
    public string Password { get; set; }

    public string Username { get; set; }
    public List<TarefaDto> Tarefas { get; set; }


    public UsuarioDto() { }

    public UsuarioDto(int id, string nome, string cargo, string username, string password)
    {
        Id = id;
        Nome = nome;
        Cargo = cargo;
        Username = username;
    }

    public UsuarioDto(Usuario usuario)

    {
        Id = usuario.Id;
        Nome = usuario.Nome;
        Cargo = usuario.Cargo;
        Username = usuario.Username;
        Password = usuario.Password;
    }
}
