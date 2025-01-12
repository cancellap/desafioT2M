using Npgsql;

namespace GerenciadorDeProjetos.Data.Infrastructure
{
    public class PostgresDbContext : IDisposable
    {
        public NpgsqlConnection Connection { get; }

        public PostgresDbContext()
        {
            Connection = new NpgsqlConnection("Host = localhost; Port = 5432; Username = postgres; Password = 1234; Database = DesafioT2M");
            Connection.Open();
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}


