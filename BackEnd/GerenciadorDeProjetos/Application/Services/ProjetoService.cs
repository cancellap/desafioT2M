﻿using GerenciadorDeProjetos.Domain.Entities;
using GerenciadorDeProjetos.Infrastructure.Interfaces;
using Newtonsoft.Json.Linq;

namespace GerenciadorDeProjetos.Application.Services
{
    public class ProjetoService
    {
        private readonly ProjetoRepository _projetoRepository;
        private readonly TokenService _tokenService;

        public ProjetoService(ProjetoRepository projetoRepository, TokenService tokenService)
        {
            _projetoRepository = projetoRepository;
            _tokenService = tokenService;
        }

        public ProjetoDto AdicionarProjeto(Projeto projeto, string token)
        {
            try
            {
                int? id = _tokenService.GetIdToken(token);
                if (id.HasValue)
                {
                    projeto.UsuarioId = id.Value;
                    ProjetoDto projetoDto = _projetoRepository.Add(projeto);
                    return projetoDto;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
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

        public IEnumerable<ProjetoDto> GetAllProjetoPorUsuario(string token)
        {

            try
            {
                int? id = _tokenService.GetIdToken(token);

                return _projetoRepository.GetAllProjetoPorUsuario(id.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter projetos: {ex.Message}");
                return new List<ProjetoDto>();
            }
        }

        public bool AtualizarProjeto(int id, string nome, string descricao, DateTime dataInicio, DateTime dataTermino, string token)
        {
            try
            {
                int idUsuario = _tokenService.GetIdToken(token);

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
                    return false;
                }

                if (idUsuario != projeto.UsuarioId)
                {
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



        public bool ExcluirProjeto(int id, string token)
        {
            try
            {
                int idUsuario = _tokenService.GetIdToken(token);
                var projeto = _projetoRepository.GetById(id);

                if (idUsuario == projeto.UsuarioId)
                {
                return _projetoRepository.Delete(id);       
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir projeto: {ex.Message}");
                return false;
            }
        }
    }
}
