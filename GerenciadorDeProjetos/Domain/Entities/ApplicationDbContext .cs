using Microsoft.EntityFrameworkCore;
using Npgsql;
using GerenciadorDeProjetos.Domain.Entities;

namespace GerenciadorDeProjetos.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public NpgsqlConnection Connection { get; private set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Connection = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=1234;Database=DesafioT2M");
            Connection.Open();
        }

        public NpgsqlCommand CreateCommand(string commandText)
        {
            var command = Connection.CreateCommand();
            command.CommandText = commandText;
            return command;
        }

        public async Task ExecuteNonQueryAsync(string commandText)
        {
            using (var command = CreateCommand(commandText))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

        public NpgsqlConnection GetConnection()
        {
            return Connection;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public new void Dispose()
        {
            Connection?.Dispose();
            base.Dispose();
        }
    }
}
