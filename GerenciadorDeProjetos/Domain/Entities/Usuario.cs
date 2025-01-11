namespace GerenciadorDeProjetos.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cargo { get; set; }

        public Usuario()
        {
        }

        public Usuario(int id, string nome, string cargo)
        {
            Id = id;
            Nome = nome;
            Cargo = cargo;
        }
    }
}
