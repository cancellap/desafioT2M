using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace GerenciadorDeProjetos.Domain.Entities
{
    public class DbContext : IDisposable
    {
       public NpgsqlConnection Connection { get; set; }

       public DbContext() {
            Connection = new NpgsqlConnection("Host = localhost; Port = 5432; Username = postgres; Password = 1234; Database = DesafioT2M");
            Connection.Open();
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}