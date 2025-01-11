namespace GerenciadorDeProjetos.Domain.Entities
{
    public class Projeto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public List<Tarefa> Tarefas { get; set; } = new List<Tarefa>();

        public Projeto()
        {
        }

        public Projeto(string nome, string? descricao, DateTime dataInicio, DateTime dataTermino, List<Tarefa> tarefas)
        {
            Nome = nome;
            Descricao = descricao;
            DataInicio = dataInicio;
            DataTermino = dataTermino;
            Tarefas = tarefas ?? new List<Tarefa>();
        }
    }
}
