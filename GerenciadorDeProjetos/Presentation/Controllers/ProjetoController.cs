using GerenciadorDeProjetos.Domain.Entities;
using GerenciadorDeProjetos.Domain.Interface;
using GerenciadorDeProjetos.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace GerenciadorDeProjetos.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetoController : ControllerBase
    {
        private readonly ProjetoService _projetoService = new ProjetoService(new ProjetoRepository());

        [HttpPost]
        public IActionResult AdicionarProjeto([FromBody] Projeto projeto)
        {
            try
            {
                if (projeto == null || string.IsNullOrWhiteSpace(projeto.Nome) || projeto.DataInicio == default(DateTime) || projeto.DataTermino == default(DateTime))
                {
                    return BadRequest("Nome, Data Início e Data Término são obrigatórios.");
                }

                bool sucesso = _projetoService.AdicionarProjeto(projeto.Nome, projeto.Descricao, projeto.DataInicio, projeto.DataTermino, projeto.Tarefas);

                if (sucesso)
                {
                    return Ok(new { message = "Projeto adicionado com sucesso!" });
                }
                else
                {
                    return StatusCode(500, new { message = "Falha ao adicionar o projeto." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public IActionResult ObterProjetoPorId(int id)
        {
            try
            {
                var projeto = _projetoService.ObterProjetoPorId(id);
                if (projeto == null)
                {
                    return NotFound(new { message = "Projeto não encontrado." });
                }
                return Ok(projeto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro: {ex.Message}" });
            }
        }

        [HttpGet]
        public IActionResult ObterTodosProjetos()
        {
            try
            {
                var projetos = _projetoService.ObterTodosProjetos();
                return Ok(projetos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarProjeto(int id, [FromBody] Projeto projeto)
        {
            try
            {
                if (projeto == null)
                {
                    return BadRequest(new { message = "Dados do projeto não podem ser nulos." });
                }

                bool sucesso = _projetoService.AtualizarProjeto(id, projeto.Nome, projeto.Descricao, projeto.DataInicio, projeto.DataTermino);

                if (sucesso)
                {
                    return Ok(new { message = "Projeto atualizado com sucesso!" });
                }

                return StatusCode(500, new { message = "Falha ao atualizar o projeto." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro no controlador: {ex.Message}" });
            }
        }


        [HttpDelete("{id}")]
        public IActionResult ExcluirProjeto(int id)
        {
            try
            {
                var projeto =  _projetoService.ObterProjetoPorId(id);
                if (projeto == null)
                {
                    return NotFound(new { message = "Projeto não encontrado." });
                }
                if (projeto.Tarefas != null && projeto.Tarefas.Any())
                {
                    return BadRequest(new { message = "Não é possível deletar projetos que possuem tarefas associadas." });
                }

                bool sucesso = _projetoService.ExcluirProjeto(id);
                if (sucesso)
                {
                    return Ok(new { message = "Projeto excluído com sucesso!" });
                }
                else
                {
                    return StatusCode(500, new { message = "Falha ao excluir o projeto." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro: {ex.Message}" });
            }
        }
    }
}
