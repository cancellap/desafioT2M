using System.ComponentModel.DataAnnotations.Schema;

public class TarefaInsertDto
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public DateTime Prazo { get; set; } 
    public string Status { get; set; }
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
        Status = tarefa.Status;
        UsuarioId = tarefa.UsuarioId;
        ProjetoId = tarefa.ProjetoId;
    }
}
