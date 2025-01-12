using GerenciadorDeProjetos.Domain.Entities;
using GerenciadorDeProjetos.Domain.Interface;
using System;
using System.Collections.Generic;

namespace GerenciadorDeProjetos.Domain.Services 
{
    public class ProjetoService
    {
        private readonly ProjetoRepository _projetoRepository;

        public ProjetoService(ProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        public bool AdicionarProjeto(string nome, string descricao, DateTime dataInicio, DateTime dataTermino, List<Tarefa> tarefas)
        {
            try
            {
                var projeto = new Projeto
                {
                    Nome = nome,
                    Descricao = descricao,
                    DataInicio = dataInicio,
                    DataTermino = dataTermino,
                    Tarefas = tarefas ?? new List<Tarefa>()
                };

                return _projetoRepository.Add(projeto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar projeto: {ex.Message}");
                return false;
            }
        }

        public ProjetoDto ObterProjetoPorId(int id)
        {
            try
            {
                var projeto = _projetoRepository.GetById(id);
                return projeto; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter projeto: {ex.Message}");
                return null;
            }
        }


        public IEnumerable<ProjetoDto> ObterTodosProjetos()
        {
            try
            {
                return _projetoRepository.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter projetos: {ex.Message}");
                return new List<ProjetoDto>();
            }
        }

        public bool AtualizarProjeto(int id, string nome, string descricao, DateTime dataInicio, DateTime dataTermino)
        {
            try
            {
                if (id <= 0)
                {
                    Console.WriteLine("ID inválido." + id);
                    return false;
                }

                if (dataInicio == DateTime.MinValue || dataTermino == DateTime.MinValue)
                {
                    Console.WriteLine("Datas inválidas.");
                    return false;
                }

                var projeto = new Projeto(_projetoRepository.GetById(id));

                if (projeto == null)
                {
                    Console.WriteLine($"Projeto com ID {id} não encontrado.");
                    return false;
                }

                projeto.Id = id;
                projeto.Nome = nome;
                projeto.Descricao = descricao;
                projeto.DataInicio = dataInicio;
                projeto.DataTermino = dataTermino;

                var atualizado = _projetoRepository.Update(projeto);
                Console.WriteLine($"Atualização no serviço: {atualizado}");
                return atualizado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar projeto no serviço: {ex.Message}");
                return false;
            }
        }



        public bool ExcluirProjeto(int id)
        {
            try
            {
                return _projetoRepository.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir projeto: {ex.Message}");
                return false;
            }
        }
    }
}
