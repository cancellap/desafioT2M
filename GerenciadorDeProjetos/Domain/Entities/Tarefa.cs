using GerenciadorDeProjetos.Domain.Entities;

public class Tarefa
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public DateTime Prazo { get; set; }
    public string Status { get; set; }
    public Usuario Responsavel { get; set; }
    public int ProjetoId { get; set; }
    public Projeto? Projeto { get; set; }

    public Tarefa() { }

    public Tarefa(string nome, string descricao, DateTime prazo, string status, Usuario responsavel, Projeto projeto)
    {
        Nome = nome;
        Descricao = descricao;
        Prazo = prazo;
        Status = status;
        Responsavel = responsavel ?? throw new ArgumentNullException(nameof(responsavel), "Responsável não pode ser nulo");
        Projeto = projeto ?? throw new ArgumentNullException(nameof(projeto), "Projeto não pode ser nulo");
    }
}
