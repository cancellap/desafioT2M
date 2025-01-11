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

        // Injeção de dependência do UsuarioService
        public UsuarioController()
        {
            _usuarioService = new UsuarioService(new UsuarioRepository());
        }

        // Adicionar um novo usuário
        [HttpPost]
        public IActionResult AdicionarUsuario([FromBody] Usuario usuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuario.Nome) || string.IsNullOrWhiteSpace(usuario.Cargo))
                {
                    return BadRequest("Nome e Cargo são obrigatórios.");
                }

                bool sucesso = _usuarioService.AdicionarUsuario(usuario.Nome, usuario.Cargo);

                if (sucesso)
                {
                    return Ok(new { message = "Usuário adicionado com sucesso!" });
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

        // Obter um usuário por ID
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
