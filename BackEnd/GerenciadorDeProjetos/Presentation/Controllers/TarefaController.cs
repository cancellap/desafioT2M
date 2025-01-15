using GerenciadorDeProjetos.Domain.Services;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult AdicionarTarefa([FromBody] TarefaInsertDto tarefaInsertDto)
        {
            try
            {
                var authorizationHeader = Request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    return Unauthorized(new { message = "Token de autorização não fornecido." });
                }

                var token = authorizationHeader.Substring(7).Trim();

                if (tarefaInsertDto == null)
                {
                    return BadRequest(new { message = "Os dados da tarefa não podem ser nulos." });
                }

                if (string.IsNullOrWhiteSpace(tarefaInsertDto.Nome) ||
                    string.IsNullOrWhiteSpace(tarefaInsertDto.Descricao) ||
                    tarefaInsertDto.Prazo == default(DateTime))
                {
                    return BadRequest(new { message = "Nome, descrição e prazo são obrigatórios." });
                }

                var tarefa = _tarefaService.AdicionarTarefa(tarefaInsertDto, token);

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
        public IActionResult AtualizarTarefa(int id, [FromBody] TarefaInsertDto tarefa)
        {
            try
            {
                if (tarefa == null)
                {
                    return BadRequest("Dados da tarefa não podem ser nulos.");
                }

                var authorizationHeader = Request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    return Unauthorized(new { message = "Token de autorização não fornecido." });
                }

                var token = authorizationHeader.Substring(7).Trim();

                var tarefaAtt = _tarefaService.AtualizarTarefa(id ,tarefa, token);

                if (tarefaAtt != null)
                {
                    return Ok(tarefaAtt);
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
