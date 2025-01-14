using Dapper;
using GerenciadorDeProjetos.Infrastructure.Data;
using GerenciadorDeProjetos.Domain.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using GerenciadorDeProjetos.Application.DTOs;

namespace GerenciadorDeProjetos.Infrastructure.Interfaces
{
    public class TarefaRepository
    {
        public Tarefa? Add(TarefaInsertDto tarefaInsertDto, int usuarioId)
        {
            try
            {
                using var conn = new PostgresDbContext().Connection;

                string query = @"
                    INSERT INTO tarefa (nome, descricao, prazo, status_tarefa, usuario_id, projeto_id)
                    VALUES (@nome, @descricao, @prazo, @statusTarefa, @usuarioId, @projetoId)
                    RETURNING id;";

                var id = conn.ExecuteScalar<int>(query, new
                {
                    nome = tarefaInsertDto.Nome,
                    descricao = tarefaInsertDto.Descricao,
                    prazo = tarefaInsertDto.Prazo,
                    statusTarefa = tarefaInsertDto.StatusTarefa.ToString(),
                    usuarioId,
                    projetoId = tarefaInsertDto.ProjetoId
                });
                Console.WriteLine(usuarioId);
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
                    StatusTarefa = tarefaInsertDto.StatusTarefa,
                    UsuarioId = usuarioId,
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
                status_tarefa AS StatusTarefa, 
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
                                status_tarefa AS StatusTarefa, 
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
                                 SET nome = @nome, descricao = @descricao, prazo = @prazo, status_tarefa = @statusTarefa, 
                                     usuario_id = @usuarioId, projeto_id = @projetoId 
                                 WHERE id = @id";

                var result = conn.Execute(query, new
                {
                    id = tarefa.Id,
                    nome = tarefa.Nome,
                    descricao = tarefa.Descricao,
                    prazo = tarefa.Prazo,
                    statusTarefa = tarefa.StatusTarefa,
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
