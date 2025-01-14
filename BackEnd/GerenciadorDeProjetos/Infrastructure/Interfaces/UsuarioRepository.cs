using Dapper;
using GerenciadorDeProjetos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using GerenciadorDeProjetos.Infrastructure.Data;
using GerenciadorDeProjetos.Domain.DTOs;


namespace GerenciadorDeProjetos.Infrastructure.Interfaces
{
    public class UsuarioRepository
    {

        public UsuarioDto Add(Usuario usuarioInsertDto)
        {
            try
            {
                using var conn = new PostgresDbContext().Connection;
                string query = @"INSERT INTO usuario (nome, cargo, username, password)
                         VALUES (@nome, @cargo, @username, @password) 
                         RETURNING id;";

                var id = conn.ExecuteScalar<int>(query, new
                {
                    nome = usuarioInsertDto.Nome,
                    cargo = usuarioInsertDto.Cargo,
                    username = usuarioInsertDto.Username,
                    password = usuarioInsertDto.Password
                });

                return new UsuarioDto(GetById(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inserir usuário: {ex.Message}");
                return null;
            }
        }


        public UsuarioDto GetByUsername(string username)
        {
            try
            {
                using var conn = new PostgresDbContext().Connection;
                string query = @"SELECT id, nome, cargo, username, password
                         FROM usuario
                         WHERE username = @username";

                Console.WriteLine($"Executando query para username: {username}");

                var usuario = conn.QueryFirstOrDefault<UsuarioDto>(query, new { username });

                if (usuario == null)
                {
                    Console.WriteLine("Usuário não encontrado.");
                }

                return usuario;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar usuário: {ex.Message}. StackTrace: {ex.StackTrace}");
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
                    id, 
                    nome, 
                    descricao, 
                    prazo, 
                    status_tarefa AS StatusTarefa, 
                    usuario_id AS UsuarioId, 
                    projeto_id AS ProjetoId 
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
                                id, 
                                nome, 
                                descricao, 
                                prazo, 
                                status_tarefa AS StatusTarefa, 
                                usuario_id AS UsuarioId, 
                                projeto_id AS ProjetoId 
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
