using GerenciadorDeProjetos.Domain.Entities;
using GerenciadorDeProjetos.Domain.Interface;
using GerenciadorDeProjetos.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorDeProjetos.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController()
        {
            _usuarioService = new UsuarioService(new UsuarioRepository());
        }

        [HttpPost]
        public IActionResult AdicionarUsuario([FromBody] Usuario usuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuario.Nome) || string.IsNullOrWhiteSpace(usuario.Cargo))
                {
                    return BadRequest(new { message = "Nome e Cargo são obrigatórios." });
                }

                var usuarioCriado = _usuarioService.AdicionarUsuario(usuario.Nome, usuario.Cargo);

                if (usuarioCriado != null)
                {
                    return Ok(new
                    {
                        usuario = usuarioCriado
                    });
                }
                else
                {
                    return StatusCode(500, new { message = "Falha ao adicionar o usuário." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public IActionResult ObterUsuarioPorId(int id)
        {
            try
            {
                var usuario = _usuarioService.ObterUsuarioPorId(id);
                if (usuario == null)
                {
                    return NotFound(new { message = "Usuário não encontrado." });
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro: {ex.Message}" });
            }
        }

        [HttpGet]
        public IActionResult ObterTodosUsuarios()
        {
            try
            {
                var usuarios = _usuarioService.ObterTodosUsuarios();

                if (usuarios == null || !usuarios.Any())
                {
                    return NotFound(new { message = "Nenhum usuário encontrado." });
                }

                foreach (var usuario in usuarios)
                {
                    if (usuario.Tarefas == null || !usuario.Tarefas.Any())
                    {
                        Console.WriteLine($"Usuário {usuario.Id} não tem tarefas.");
                    }
                }

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarUsuario(int id, [FromBody] Usuario usuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuario.Nome) || string.IsNullOrWhiteSpace(usuario.Cargo))
                {
                    return BadRequest("Nome e Cargo são obrigatórios.");
                }

                bool sucesso = _usuarioService.AtualizarUsuario(id, usuario.Nome, usuario.Cargo);

                if (sucesso)
                {
                    return Ok(new { message = "Usuário atualizado com sucesso!" });
                }
                else
                {
                    return StatusCode(500, new { message = "Falha ao atualizar o usuário." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult ExcluirUsuario(int id)
        {
            try
            {
                bool sucesso = _usuarioService.ExcluirUsuario(id);

                if (sucesso)
                {
                    return Ok(new { message = "Usuário excluído com sucesso!" });
                }
                else
                {
                    return StatusCode(500, new { message = "Falha ao excluir o usuário." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro: {ex.Message}" });
            }
        }
    }
}
