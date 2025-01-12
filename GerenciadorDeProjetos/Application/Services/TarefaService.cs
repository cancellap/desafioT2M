using GerenciadorDeProjetos.Domain.DTOs;
using GerenciadorDeProjetos.Domain.Entities;
using GerenciadorDeProjetos.Domain.Interface;
using System;
using System.Collections.Generic;

namespace GerenciadorDeProjetos.Domain.Services
{
    public class TarefaService
    {
        private readonly TarefaRepository _tarefaRepository;

        public TarefaService(TarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }

        public TarefaInsertDto? AdicionarTarefa(TarefaInsertDto tarefaInsertDto)
        {
            try
            {
                var tarefa = _tarefaRepository.Add(tarefaInsertDto);

                if (tarefa != null)
                {
                    return new TarefaInsertDto
                    {
                        Nome = tarefa.Nome,
                        Descricao = tarefa.Descricao,
                        Prazo = tarefa.Prazo,
                        Status = tarefa.Status,
                        UsuarioId = tarefa.UsuarioId,
                        ProjetoId = tarefa.ProjetoId
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar tarefa: {ex.Message}");
                return null;
            }
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
                    Status = tarefa.Status,
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

        public bool AtualizarTarefa(int id, string nome, string descricao, DateTime prazo, string status, int usuarioId, int projetoId)
        {
            try
            {
                var tarefa = new Tarefa(nome, descricao, prazo, status, usuarioId, projetoId) { Id = id };
                return _tarefaRepository.Update(tarefa);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar tarefa: {ex.Message}");
                return false;
            }
        }

        public bool ExcluirTarefa(int id)
        {
            try
            {
                return _tarefaRepository.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir tarefa: {ex.Message}");
                return false;
            }
        }
    }
}
