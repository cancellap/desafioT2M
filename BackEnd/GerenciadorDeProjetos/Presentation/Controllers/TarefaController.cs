using GerenciadorDeProjetos.Domain.DTOs;
using GerenciadorDeProjetos.Domain.Entities;
using GerenciadorDeProjetos.Domain.Services;
using GerenciadorDeProjetos.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GerenciadorDeProjetos.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly TarefaService _tarefaService;

        public TarefaController(TarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        [HttpPost]
        [HttpPost]
        public IActionResult AdicionarTarefa([FromBody] TarefaInsertDto tarefaDTO)
        {
            try
            {
                if (tarefaDTO == null)
                {
                    return BadRequest(new { message = "Os dados da tarefa não podem ser nulos." });
                }

                if (string.IsNullOrWhiteSpace(tarefaDTO.Nome) ||
                    string.IsNullOrWhiteSpace(tarefaDTO.Descricao) ||
                    tarefaDTO.Prazo == default(DateTime))
                {
                    return BadRequest(new { message = "Nome, descrição e prazo são obrigatórios." });
                }

                var tarefa = _tarefaService.AdicionarTarefa(tarefaDTO);

                if (tarefa != null)
                {
                    return Ok(tarefa);
                }
                else
                {
                    return StatusCode(500, new { message = "Falha ao adicionar a tarefa." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro: {ex.Message}" });
            }
        }



        [HttpGet("{id}")]
        public IActionResult ObterTarefaPorId(int id)
        {
            try
            {
                var tarefaDto = _tarefaService.ObterTarefaPorId(id); 

                if (tarefaDto == null)
                {
                    return NotFound(new { message = "Tarefa não encontrada." });
                }

                return Ok(tarefaDto); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro: {ex.Message}" });
            }
        }


        [HttpGet]
        public IActionResult ObterTodasTarefas()
        {
            try
            {
                var tarefas = _tarefaService.ObterTodasTarefas();
                return Ok(tarefas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarTarefa(int id, [FromBody] Tarefa tarefa)
        {
            try
            {
                if (tarefa == null)
                {
                    return BadRequest("Dados da tarefa não podem ser nulos.");
                }

                bool sucesso = _tarefaService.AtualizarTarefa(
                    id,
                    tarefa.Nome,
                    tarefa.Descricao,
                    tarefa.Prazo,
                    tarefa.StatusTarefa,
                    tarefa.UsuarioId,
                    tarefa.ProjetoId
                );

                if (sucesso)
                {
                    return Ok(new { message = "Tarefa atualizada com sucesso!" , tarefa});
                }
                else
                {
                    return StatusCode(500, new { message = "Falha ao atualizar a tarefa." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult ExcluirTarefa(int id)
        {
            try
            {
                var authorizationHeader = Request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    return Unauthorized(new { message = "Token de autorização não fornecido." });
                }

                var token = authorizationHeader.Substring(7).Trim();

                bool sucesso = _tarefaService.ExcluirTarefa(id, token);
                if (sucesso)
                {
                    return Ok(new { message = "Tarefa excluída com sucesso!" });
                }
                else
                {
                    return StatusCode(500, new { message = "Falha ao excluir a tarefa." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
