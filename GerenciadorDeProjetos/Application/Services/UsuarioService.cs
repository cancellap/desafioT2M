using GerenciadorDeProjetos.Domain.Entities;
using GerenciadorDeProjetos.Domain.Interface;
using System;
using System.Collections.Generic;

namespace GerenciadorDeProjetos.Domain.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public bool AdicionarUsuario(string nome, string cargo)
        {
            try
            {
                var usuario = new Usuario
                {
                    Nome = nome,
                    Cargo = cargo
                };

                return _usuarioRepository.Add(usuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar usuário: {ex.Message}");
                return false;
            }
        }

        public Usuario ObterUsuarioPorId(int id)
        {
            try
            {
                return _usuarioRepository.GetById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter usuário: {ex.Message}");
                return null;
            }
        }

        public IEnumerable<Usuario> ObterTodosUsuarios()
        {
            try
            {
                return _usuarioRepository.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter usuários: {ex.Message}");
                return new List<Usuario>();
            }
        }

        public bool AtualizarUsuario(int id, string nome, string cargo)
        {
            try
            {
                var usuario = new Usuario
                {
                    Id = id,
                    Nome = nome,
                    Cargo = cargo
                };

                return _usuarioRepository.Update(usuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar usuário: {ex.Message}");
                return false;
            }
        }

        public bool ExcluirUsuario(int id)
        {
            try
            {
                return _usuarioRepository.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir usuário: {ex.Message}");
                return false;
            }
        }
    }
}
