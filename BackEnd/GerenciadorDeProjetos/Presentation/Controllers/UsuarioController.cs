using GerenciadorDeProjetos.Domain.DTOs;
using GerenciadorDeProjetos.Domain.Entities;
using GerenciadorDeProjetos.Domain.Exceptions;
using GerenciadorDeProjetos.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorDeProjetos.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public IActionResult AdicionarUsuario([FromBody] UsuarioInsertDto usuarioInsertDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuarioInsertDto.Nome) || string.IsNullOrWhiteSpace(usuarioInsertDto.Cargo))
                {
                    return BadRequest(new { message = "Nome e Cargo são obrigatórios." });
                }

                var usuarioCriado = _usuarioService.AdicionarUsuario(usuarioInsertDto);

                if (usuarioCriado != null)
                {
                    return Ok(new
                    {
                        usuario = usuarioCriado
                    });
                }

                return BadRequest(new { message = "Não foi possível criar o usuário." });
            }
            catch (SenhasNaoCoincidemException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UsuarioJaExisteException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro interno: {ex.Message}" });
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


        [HttpGet("username/{username}")]
        public IActionResult GetByUsername(string username)
        {
            var usuario = _usuarioService.GetByUsername(username);

            if (usuario == null)
            {
                return Ok();
            }

            return Ok(usuario);
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
