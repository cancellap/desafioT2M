using System.ComponentModel.DataAnnotations.Schema;
using GerenciadorDeProjetos.Domain.Enum;

public class TarefaInsertDto
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public DateTime Prazo { get; set; }
    [Column("status_tarefa")]
    public StatusTarefa StatusTarefa { get; set; }
    [Column("usuario_id")]
    public int UsuarioId { get; set; }
    [Column("projeto_id")]
    public int ProjetoId { get; set; }

    public TarefaInsertDto()
    {
    }

    public TarefaInsertDto(Tarefa tarefa) 
    {
        if (tarefa == null)
        {
            throw new ArgumentNullException(nameof(tarefa), "A tarefa não pode ser nula.");
        }

        Nome = tarefa.Nome;
        Descricao = tarefa.Descricao;
        Prazo = tarefa.Prazo;
        StatusTarefa = tarefa.StatusTarefa;
        UsuarioId = tarefa.UsuarioId;
        ProjetoId = tarefa.ProjetoId;
    }
}
