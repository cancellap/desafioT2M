using System.Collections.Generic;

namespace GerenciadorDeProjetos.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cargo { get; set; }
        public List<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
        public string Username { get; set; }
        public string Password { get; set; }

        public Usuario()
        {
        }

        public Usuario(UsuarioDto usuarioDto)
        {
            Id = usuarioDto.Id; 
            Nome = usuarioDto.Nome;
            Cargo = usuarioDto.Cargo;
            Tarefas = usuarioDto.Tarefas?.Select(t => new Tarefa(t)).ToList() ?? new List<Tarefa>(); 
        }

        public Usuario(int id, string nome, string cargo,string username)
        {
            Id = id;
            Nome = nome;
            Cargo = cargo;
            Username= username;
        }
    }
}
