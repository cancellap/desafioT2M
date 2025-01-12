using Dapper;
using GerenciadorDeProjetos.Data.Infrastructure;
using GerenciadorDeProjetos.Domain.DTOs;
using GerenciadorDeProjetos.Domain.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GerenciadorDeProjetos.Domain.Interface
{
    public class TarefaRepository
    {
        public Tarefa? Add(TarefaInsertDto tarefaInsertDto)
        {
            try
            {
                using var conn = new PostgresDbContext().Connection;

                string query = @"
            INSERT INTO tarefa (nome, descricao, prazo, status, usuario_id, projeto_id)
            VALUES (@nome, @descricao, @prazo, @status, @usuarioId, @projetoId)
            RETURNING id;";

                var id = conn.ExecuteScalar<int>(query, new
                {
                    nome = tarefaInsertDto.Nome,
                    descricao = tarefaInsertDto.Descricao,
                    prazo = tarefaInsertDto.Prazo,
                    status = tarefaInsertDto.Status,
                    usuarioId = tarefaInsertDto.UsuarioId,
                    projetoId = tarefaInsertDto.ProjetoId
                });

                if (id <= 0)
                {
                    Console.WriteLine("Erro: ID retornado é inválido.");
                    return null;
                }

                return new Tarefa
                {
                    Id = id,
                    Nome = tarefaInsertDto.Nome,
                    Descricao = tarefaInsertDto.Descricao,
                    Prazo = tarefaInsertDto.Prazo,
                    Status = tarefaInsertDto.Status,
                    UsuarioId = tarefaInsertDto.UsuarioId,
                    ProjetoId = tarefaInsertDto.ProjetoId
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inserir tarefa: {ex.Message}");
                return null;
            }
        }



        public TarefaDto GetById(int id)
        {
            try
            {
                using var conn = new PostgresDbContext().Connection;

                string query = @"
            SELECT 
                id, 
                nome, 
                descricao, 
                prazo, 
                status, 
                usuario_id AS UsuarioId, 
                projeto_id AS ProjetoId 
            FROM tarefa 
            WHERE id = @id";

                Tarefa tarefa = conn.QueryFirstOrDefault<Tarefa>(query, new { id });

                return tarefa != null ? new TarefaDto(tarefa) : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter tarefa: {ex.Message}");
                return null;
            }
        }


        public IEnumerable<Tarefa> GetAll()
        {
            try
            {
                using var conn = new PostgresDbContext().Connection;
                string query = @"
                            SELECT 
                                id, 
                                nome, 
                                descricao, 
                                prazo, 
                                status, 
                                usuario_id AS UsuarioId, 
                                projeto_id AS ProjetoId
                            FROM tarefa";

                return conn.Query<Tarefa>(query).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter tarefas: {ex.Message}");
                return new List<Tarefa>();
            }
        }

        public bool Update(Tarefa tarefa)
        {
            try
            {
                using var conn = new PostgresDbContext().Connection;
                string query = @"UPDATE tarefa 
                                 SET nome = @nome, descricao = @descricao, prazo = @prazo, status = @status, 
                                     usuario_id = @usuarioId, projeto_id = @projetoId 
                                 WHERE id = @id";

                var result = conn.Execute(query, new
                {
                    id = tarefa.Id,
                    nome = tarefa.Nome,
                    descricao = tarefa.Descricao,
                    prazo = tarefa.Prazo,
                    status = tarefa.Status,
                    usuarioId = tarefa.UsuarioId,
                    projetoId = tarefa.ProjetoId,
                });

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar tarefa: {ex.Message}");
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using var conn = new PostgresDbContext().Connection;
                string query = @"DELETE FROM tarefa WHERE id = @id";

                var result = conn.Execute(query, new { id });

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir tarefa: {ex.Message}");
                return false;
            }
        }
    }
}
