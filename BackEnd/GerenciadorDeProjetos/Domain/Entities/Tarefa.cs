using System.ComponentModel.DataAnnotations.Schema;
using GerenciadorDeProjetos.Application.DTOs;
using GerenciadorDeProjetos.Domain.Entities;
using GerenciadorDeProjetos.Domain.Enum;

public class Tarefa
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public DateTime Prazo { get; set; }
    [Column("status_tarefa")]
    public StatusTarefa StatusTarefa { get; set; }
    public Usuario? Responsavel { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [Column("projeto_id")]
    public int ProjetoId { get; set; }

    public Projeto? Projeto { get; set; }

    public Tarefa() { }

    public Tarefa(int id, string nome, string descricao, DateTime prazo, StatusTarefa status, int usuarioId, int projetoId)
    {
        Id = id;
        Nome = nome;
        Descricao = descricao;
        Prazo = prazo;
        StatusTarefa = status;
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
        StatusTarefa = tarefaInsertDto.StatusTarefa;
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
        StatusTarefa = Enum.Parse<StatusTarefa>(tarefaDto.StatusTarefa.ToString());
        UsuarioId = tarefaDto.UsuarioId;
        ProjetoId = tarefaDto.ProjetoId;
    }


}
