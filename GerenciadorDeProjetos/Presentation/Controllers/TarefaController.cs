using GerenciadorDeProjetos.Domain.Entities;
using GerenciadorDeProjetos.Domain.Interface;
using GerenciadorDeProjetos.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GerenciadorDeProjetos.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly TarefaService _tarefaService = new TarefaService(new TarefaRepository());

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
                    return Ok(new
                    {
                        message = "Tarefa adicionada com sucesso!",
                        tarefa = tarefa
                    });
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
                var tarefa = _tarefaService.ObterTarefaPorId(id);
                if (tarefa == null)
                {
                    return NotFound(new { message = "Tarefa não encontrada." });
                }
                return Ok(tarefa);
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
                    tarefa.Status,
                    tarefa.UsuarioId,
                    tarefa.ProjetoId
                );

                if (sucesso)
                {
                    return Ok(new { message = "Tarefa atualizada com sucesso!" });
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
                bool sucesso = _tarefaService.ExcluirTarefa(id);
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
                return StatusCode(500, new { message = $"Erro: {ex.Message}" });
            }
        }
    }
}
