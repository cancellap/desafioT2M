using System.ComponentModel.DataAnnotations.Schema;
using GerenciadorDeProjetos.Domain.DTOs;
using GerenciadorDeProjetos.Domain.Entities;

public class Tarefa
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public DateTime Prazo { get; set; }
    public string Status { get; set; }
    public Usuario? Responsavel { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [Column("projeto_id")]
    public int ProjetoId { get; set; }

    public Projeto? Projeto { get; set; }

    public Tarefa() { }

    public Tarefa(string nome, string descricao, DateTime prazo, string status, int usuarioId, int projetoId)
    {
        Nome = nome;
        Descricao = descricao;
        Prazo = prazo;
        Status = status;
        UsuarioId = usuarioId;
        ProjetoId = projetoId;
    }
    public Tarefa(TarefaInsertDto tarefaInsertDto)
    {
        if (tarefaInsertDto == null)
        {
            throw new ArgumentNullException(nameof(tarefaInsertDto), "O DTO da tarefa não pode ser nulo.");
        }

        Nome = tarefaInsertDto.Nome;
        Descricao = tarefaInsertDto.Descricao;
        Prazo = tarefaInsertDto.Prazo;
        Status = tarefaInsertDto.Status;
        UsuarioId = tarefaInsertDto.UsuarioId; 
        ProjetoId = tarefaInsertDto.ProjetoId;
    }
    public Tarefa(TarefaDto tarefaDto)
    {
        if (tarefaDto == null)
        {
            throw new ArgumentNullException(nameof(tarefaDto), "O DTO da tarefa não pode ser nulo.");
        }
        Id = tarefaDto.Id;
        Nome = tarefaDto.Nome;
        Descricao = tarefaDto.Descricao;
        Prazo = DateTime.TryParse(tarefaDto.Prazo, out var prazo) ? prazo.Date : DateTime.MinValue;
        Status = tarefaDto.Status;
        UsuarioId = tarefaDto.UsuarioId;
        ProjetoId = tarefaDto.ProjetoId;
    }


}
