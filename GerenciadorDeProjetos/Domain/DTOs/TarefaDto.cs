using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorDeProjetos.Domain.DTOs
{
    public class TarefaDto

    {
        public int Id { get; set; } 
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public String Prazo { get; set; }
        public string Status { get; set; }
        [Column("usuario_id")]
        public int UsuarioId { get; set; }
        [Column("projeto_id")]
        public int ProjetoId { get; set; }

        public TarefaDto()
        {
            
        }

        public TarefaDto(Tarefa tarefa)
        {
            if (tarefa == null)
            {
                throw new ArgumentNullException(nameof(tarefa), "A tarefa não pode ser nula.");
            }
            Id = tarefa.Id;
            Nome = tarefa.Nome;
            Descricao = tarefa.Descricao;
            Prazo = tarefa.Prazo.ToString("yyyy-MM-dd");
            Status = tarefa.Status;
            UsuarioId = tarefa.UsuarioId;
            ProjetoId = tarefa.ProjetoId;
        }

    }
}
