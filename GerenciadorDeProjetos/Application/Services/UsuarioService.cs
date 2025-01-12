using System.Linq;
using GerenciadorDeProjetos.Domain.DTOs;
using GerenciadorDeProjetos.Domain.Entities;
using GerenciadorDeProjetos.Domain.Interface;

namespace GerenciadorDeProjetos.Domain.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public UsuarioDto AdicionarUsuario(string nome, string cargo)
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
                return null;
            }
        }
        public UsuarioDto ObterUsuarioPorId(int id)
        {
            try
            {
                var usuario = _usuarioRepository.GetById(id);

                if (usuario != null)
                {
                    var tarefasDto = usuario.Tarefas.Select(t => new TarefaDto(t)).ToList();

                    return new UsuarioDto(usuario)
                    {
                        Tarefas = tarefasDto
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter usuário com tarefas: {ex.Message}");
                return null;
            }
        }

        public IEnumerable<UsuarioDto> ObterTodosUsuarios()
        {
            try
            {
                var usuarios = _usuarioRepository.GetAll();
                var usuariosDto = new List<UsuarioDto>();

                foreach (var usuario in usuarios)
                {
                    var tarefasDto = _usuarioRepository.ObterTarefasPorUsuario(usuario.Id)
                                    .Select(t => new TarefaDto(t))
                                    .ToList();

                    if (!tarefasDto.Any())
                    {
                        Console.WriteLine($"Usuário {usuario.Id} não tem tarefas associadas.");
                    }

                    var usuarioDto = new UsuarioDto(usuario)
                    {
                        Tarefas = tarefasDto
                    };

                    usuariosDto.Add(usuarioDto);
                }

                return usuariosDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter usuários com tarefas: {ex.Message}");
                return new List<UsuarioDto>();
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
