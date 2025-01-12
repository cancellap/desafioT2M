using Dapper;
using GerenciadorDeProjetos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using GerenciadorDeProjetos.Data.Infrastructure;

namespace GerenciadorDeProjetos.Domain.Interface
{
    public class UsuarioRepository
    {
        public object JsonConvert { get; private set; }

        public UsuarioDto Add(Usuario usuario)
        {
            try
            {
                using var conn = new PostgresDbContext().Connection;
                string query = @"INSERT INTO usuario (nome, cargo)
                         VALUES (@nome, @cargo) 
                         RETURNING id;";

                var id = conn.ExecuteScalar<int>(query, new { nome = usuario.Nome, cargo = usuario.Cargo });

                return new UsuarioDto(GetById(id)) ;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inserir usuário: {ex.Message}");
                return null;
            }
        }
        public List<Tarefa> ObterTarefasPorUsuario(int usuarioId)
        {
            using var conn = new PostgresDbContext().Connection;
            string query = @"SELECT * FROM tarefa WHERE usuario_id = @usuarioId";

            var tarefas = conn.Query<Tarefa>(query, new { usuarioId }).ToList();
            return tarefas;
        }


    

        public Usuario GetById(int id)
        {
            try
            {
                using var conn = new PostgresDbContext().Connection;

                string query = @"SELECT * FROM usuario WHERE id = @id";
                var usuario = conn.QueryFirstOrDefault<Usuario>(query, new { id });

                if (usuario != null)
                {
                    string tarefasQuery = @"
                SELECT 
                    T.Id, 
                    T.Nome, 
                    T.Descricao, 
                    T.Prazo, 
                    T.Status, 
                    T.Usuario_Id AS UsuarioId, 
                    T.Projeto_Id AS ProjetoId
                FROM Tarefa T
                WHERE T.Usuario_Id = @id";

                    var tarefas = conn.Query<Tarefa>(tarefasQuery, new { id }).ToList();
                    usuario.Tarefas = tarefas; 

                    foreach (var tarefa in tarefas)
                    {
                        Console.WriteLine($"Tarefa - UsuarioId: {tarefa.UsuarioId}, ProjetoId: {tarefa.ProjetoId}, Prazo: {tarefa.Prazo}");
                    }
                }

                return usuario; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter usuário: {ex.Message}");
                return null;
            }
        }


        public IEnumerable<Usuario> GetAll()
        {
            try
            {
                using var conn = new PostgresDbContext().Connection;

                string query = @"SELECT * FROM usuario";
                var usuarios = conn.Query<Usuario>(query).ToList();

                Console.WriteLine($"Número de usuários encontrados: {usuarios.Count}");

                foreach (var usuario in usuarios)
                {
                    string tarefasQuery = @"
            SELECT 
                T.Id, 
                T.Nome, 
                T.Descricao, 
                T.Prazo, 
                T.Status, 
                T.Usuario_Id AS UsuarioId, 
                T.Projeto_Id AS ProjetoId
            FROM Tarefa T
            WHERE T.Usuario_Id = @id";

                    var tarefas = conn.Query<Tarefa>(tarefasQuery, new { id = usuario.Id }).ToList();
                    usuario.Tarefas = tarefas;

                    Console.WriteLine($"Usuário: {usuario.Id}, Tarefas Encontradas: {tarefas.Count}");
                }


                return usuarios;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter usuários: {ex.Message}");
                return new List<Usuario>();
            }
        }




        public bool Update(Usuario usuario)
        {
            try
            {
                using var conn = new PostgresDbContext().Connection;
                string query = @"UPDATE usuario 
                                 SET nome = @nome, cargo = @cargo 
                                 WHERE id = @id";

                var result = conn.Execute(query, new { nome = usuario.Nome, cargo = usuario.Cargo, id = usuario.Id });

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar usuário: {ex.Message}");
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using var conn = new PostgresDbContext().Connection;
                string query = @"DELETE FROM usuario WHERE id = @id";

                var result = conn.Execute(query, new { id });

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir usuário: {ex.Message}");
                return false;
            }
        }
    }
}
