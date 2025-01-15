using GerenciadorDeProjetos.Application.DTOs;
using GerenciadorDeProjetos.Domain.Enum;
using GerenciadorDeProjetos.Infrastructure.Interfaces;


namespace GerenciadorDeProjetos.Domain.Services
{
    public class TarefaService
    {
        private readonly TarefaRepository _tarefaRepository;
        private readonly TokenService _tokenService;
        private readonly RabbitMqService _rabbitMqService;
        private readonly TimeSpan _intervaloVerificacao = TimeSpan.FromMinutes(1);

        public TarefaService(TarefaRepository tarefaRepository, RabbitMqService rabbitMqService, TokenService tokenService)
        {
            _tarefaRepository = tarefaRepository;
            _rabbitMqService = rabbitMqService;
            _tokenService = tokenService;
        }
        public TarefaDto? AdicionarTarefa(TarefaInsertDto tarefaInsertDto, string token)
        {
            try
            {
                int id = _tokenService.GetIdToken(token);

                var tarefa = _tarefaRepository.Add(tarefaInsertDto, id);

                if (tarefa != null)
                {
                    Task.Run(() => IniciarContagemRegressiva(tarefa));
                    return new TarefaDto(tarefa);

                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar tarefa: {ex.Message}");
                return null;
            }
        }

        private void IniciarContagemRegressiva(Tarefa tarefa)
        {
            var prazoRestante = tarefa.Prazo - DateTime.Now;

            while (prazoRestante > TimeSpan.Zero)
            {
                var mensagemContagem = $"Tarefa com ID {tarefa.Id}: Faltam {prazoRestante.Days} dias," +
                    $" {prazoRestante.Hours} horas, {prazoRestante.Minutes} minutos para o prazo e " +
                    $"{prazoRestante.Seconds} segundos para o prazo.";
                _rabbitMqService.SendMessage(mensagemContagem);
                Console.WriteLine(mensagemContagem);

                System.Threading.Thread.Sleep(_intervaloVerificacao);

                prazoRestante = tarefa.Prazo - DateTime.Now;
            }

            var mensagemPrazoAlcancado = $"Tarefa com ID {tarefa.Id}: O prazo foi alcançado!";
            _rabbitMqService.SendMessage(mensagemPrazoAlcancado);
        }

        public TarefaDto ObterTarefaPorId(int id)
        {
            try
            {
                return _tarefaRepository.GetById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter tarefa: {ex.Message}");
                return null;
            }
        }

        public IEnumerable<TarefaDto> ObterTodasTarefas()
        {
            try
            {
                var tarefas = _tarefaRepository.GetAll();

                var tarefasDTO = tarefas.Select(tarefa => new TarefaDto
                {
                    Id = tarefa.Id,
                    Nome = tarefa.Nome,
                    Descricao = tarefa.Descricao,
                    Prazo = tarefa.Prazo.ToString("yyyy-MM-dd"),
                    StatusTarefa = tarefa.StatusTarefa.ToString(),
                    UsuarioId = tarefa.UsuarioId,
                    ProjetoId = tarefa.ProjetoId
                }).ToList();

                return tarefasDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter tarefas: {ex.Message}");
                return new List<TarefaDto>();
            }
        }
        public TarefaDto AtualizarTarefa(int id, TarefaInsertDto tarefaInsertDto, string token)
        {
            try
            {
                int idUsuarioToken = _tokenService.GetIdToken(token);

                var tarefa = _tarefaRepository.GetById(id);
                if (tarefa.UsuarioId != idUsuarioToken)
                {
                    throw new Exception("Só é possível editar tarefas em que você seja responsável.");
                }


                var tarefaAntiga = _tarefaRepository.GetById(id);

                if (tarefaAntiga == null)
                {
                    throw new Exception($"Tarefa com ID {id} não encontrada.");
                }

                if (tarefaAntiga.StatusTarefa != tarefaInsertDto.StatusTarefa.ToString())
                {
                    var mensagem = $"Tarefa com ID {id} teve o status alterado de '{tarefaAntiga.StatusTarefa}' para '{tarefaInsertDto.StatusTarefa.ToString()}'";
                    _rabbitMqService.SendMessage(mensagem);
                }

                var tarefaNew = new Tarefa(id, tarefaInsertDto.Nome,
                    tarefaInsertDto.Descricao,
                    tarefaInsertDto.Prazo,
                    tarefaInsertDto.StatusTarefa,
                    tarefaInsertDto.UsuarioId,
                    tarefaInsertDto.ProjetoId);

                var tarefaAtualizada = _tarefaRepository.Update(tarefaNew);


                if (tarefaAtualizada == null)
                {
                    throw new Exception("Erro ao salvar atualização no banco de dados.");
                }

                return new TarefaDto(tarefaAtualizada);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar tarefa: {ex.Message}");
                throw;
            }
        }



        public bool ExcluirTarefa(int id, string token)
        {
            try
            {
                int idUsuarioToken = _tokenService.GetIdToken(token);

                var tarefa = _tarefaRepository.GetById(id);
                if (tarefa.UsuarioId != idUsuarioToken)
                {
                    throw new Exception("Só é possível deletar tarefas em que você seja responsável.");
                }

                var mensagemContagem = $"Tarefa com ID {id} foi concluida.";
                _rabbitMqService.SendMessage(mensagemContagem);

                return _tarefaRepository.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir tarefa: {ex.Message}");
                throw;
            }
        }

    }
}
