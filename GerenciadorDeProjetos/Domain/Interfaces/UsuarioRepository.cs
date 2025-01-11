using Dapper;
using GerenciadorDeProjetos.Domain.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GerenciadorDeProjetos.Domain.Interface
{
    public class UsuarioRepository
    {
        public bool Add(Usuario usuario)
        {
            try
            {
                using var conn = new DbContext().Connection;
                string query = @"INSERT INTO usuarios (nome, cargo)
                                 VALUES (@nome, @cargo);";

                var result = conn.Execute(query, new { nome = usuario.Nome, cargo = usuario.Cargo });

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inserir usuário: {ex.Message}");
                return false;
            }
        }

        public Usuario GetById(int id)
        {
            try
            {
                using var conn = new DbContext().Connection;
                string query = @"SELECT * FROM usuarios WHERE id = @id";

                return conn.QueryFirstOrDefault<Usuario>(query, new { id });
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
                using var conn = new DbContext().Connection;
                string query = @"SELECT * FROM usuarios";

                return conn.Query<Usuario>(query).ToList();
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
                using var conn = new DbContext().Connection;
                string query = @"UPDATE usuarios 
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
                using var conn = new DbContext().Connection;
                string query = @"DELETE FROM usuarios WHERE id = @id";

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
