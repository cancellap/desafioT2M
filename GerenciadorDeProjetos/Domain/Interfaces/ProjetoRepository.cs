using System.Data;
using Dapper;
using GerenciadorDeProjetos.Data.Infrastructure;
using GerenciadorDeProjetos.Domain.Entities;

namespace GerenciadorDeProjetos.Domain.Interface
{
    public class ProjetoRepository
    {
        public bool Add(Projeto projeto)
        {
            IDbTransaction transaction = null;

            try
            {
                using var conn = new PostgresDbContext().Connection;
                transaction = conn.BeginTransaction();

                string query = @"INSERT INTO projeto (nome, descricao, data_inicio, data_termino)
                         VALUES (@nome, @descricao, @dataInicio, @dataTermino) RETURNING id;";

                var projetoId = conn.QuerySingle<int>(query, new
                {
                    nome = projeto.Nome,
                    descricao = projeto.Descricao,
                    dataInicio = projeto.DataInicio,
                    dataTermino = projeto.DataTermino,
                }, transaction: transaction);

                if (projetoId > 0 && projeto.Tarefas.Any())
                {
                    foreach (var tarefa in projeto.Tarefas)
                    {
                        string tarefaQuery = @"INSERT INTO tarefa (nome, descricao, prazo, status, usuario_id, projeto_id) 
                                       VALUES (@nome, @descricao, @prazo, @status, @usuarioId, @projetoId);";

                        conn.Execute(tarefaQuery, new
                        {
                            nome = tarefa.Nome,
                            descricao = tarefa.Descricao,
                            prazo = tarefa.Prazo,
                            status = tarefa.Status,
                            usuarioId = tarefa.UsuarioId,
                            projetoId = projetoId
                        }, transaction: transaction);
                    }
                }

                transaction.Commit();

                Console.WriteLine(projetoId > 0);
                return projetoId > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inserir projeto repo: {ex.Message}");

                transaction?.Rollback();
                return false;
            }
        }


        public ProjetoDto GetById(int id)
        {
            try
            {
                using var conn = new PostgresDbContext().Connection;
                string query = @"                  
                                SELECT 
                                    P.Id, 
                                    P.Nome, 
                                    P.Descricao, 
                                    P.data_inicio as DataInicio , 
                                    P.data_termino as DataTermino
                                FROM Projeto P
                                WHERE P.Id = @id";

                var projeto = conn.QueryFirstOrDefault<Projeto>(query, new { id });
                if (projeto != null)
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
                                        INNER JOIN Projeto P ON T.Projeto_Id = P.Id
                                        WHERE P.Id = @id";
                    var tarefas = conn.Query<Tarefa>(tarefasQuery, new { id }).ToList();

                    projeto.Tarefas = tarefas;
                }
                return projeto != null ? new ProjetoDto(projeto) : null;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public IEnumerable<ProjetoDto> GetAll()
        {
            try
            {
                using var conn = new PostgresDbContext().Connection;

                string query = @"SELECT 
                                    P.Id, 
                                    P.Nome, 
                                    P.Descricao, 
                                    P.data_inicio AS DataInicio, 
                                    P.data_termino AS DataTermino
                                FROM Projeto P";
                var projetos = conn.Query<Projeto>(query).ToList();

                foreach (var projeto in projetos)
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
                                  INNER JOIN Projeto P ON T.Projeto_Id = P.Id
                                  WHERE P.Id = @id";
                    var tarefas = conn.Query<Tarefa>(tarefasQuery, new { id = projeto.Id }).ToList();

                    projeto.Tarefas = tarefas;
                }

                var projetoDtos = projetos.Select(p => new ProjetoDto(p)).ToList();
                return projetoDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter projetos: {ex.Message}");
                return new List<ProjetoDto>();
            }
        }


        public bool Update(Projeto projeto)
        {
            try
            {
                using var conn = new PostgresDbContext().Connection;
                string query = @"UPDATE projeto
                         SET nome = @nome, descricao = @descricao, data_inicio = @dataInicio, data_termino = @dataTermino 
                         WHERE id = @id";

                Console.WriteLine($"Query: {query}");
                Console.WriteLine($"Parâmetros: Nome={projeto.Nome}, Descricao={projeto.Descricao}, DataInicio={projeto.DataInicio}, DataTermino={projeto.DataTermino}, Id={projeto.Id}");

                var result = conn.Execute(query, new
                {
                    nome = projeto.Nome,
                    descricao = projeto.Descricao,
                    dataInicio = projeto.DataInicio,
                    dataTermino = projeto.DataTermino,
                    id = projeto.Id
                });

                Console.WriteLine($"Linhas afetadas: {result}");
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar projeto: {ex.Message}");
                return false;
            }
        }



        public bool Delete(int id)
        {
            try
            {
                using var conn = new PostgresDbContext().Connection;
                string query = @"DELETE FROM projeto WHERE id = @id";

                var result = conn.Execute(query, new { id });

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir projeto: {ex.Message}");
                return false;
            }
        }
    }
}
