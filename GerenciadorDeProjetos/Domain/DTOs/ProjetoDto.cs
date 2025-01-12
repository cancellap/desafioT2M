using GerenciadorDeProjetos.Domain.DTOs;
using System.ComponentModel.DataAnnotations.Schema;

public class ProjetoDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string? Descricao { get; set; }

    [Column("data_inicio")]
    public string DataInicio { get; set; }

    [Column("data_termino")]
    public string DataTermino { get; set; }

    public List<TarefaDto> Tarefas { get; set; } = new List<TarefaDto>();

    public ProjetoDto() { }

    public ProjetoDto(Projeto projeto)
    {
        if (projeto == null)
        {
            throw new ArgumentNullException(nameof(projeto), "O projeto não pode ser nulo.");
        }

        Id = projeto.Id;
        Nome = projeto.Nome;
        Descricao = projeto.Descricao;

        DataInicio = projeto.DataInicio.ToString("yyyy-MM-dd");
        DataTermino = projeto.DataTermino.ToString("yyyy-MM-dd");

        Tarefas = projeto.Tarefas.Select(t => new TarefaDto(t)).ToList();
    }
}
