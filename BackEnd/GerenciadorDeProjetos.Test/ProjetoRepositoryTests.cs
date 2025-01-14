using System.Linq;
using GerenciadorDeProjetos.Domain.Entities;
using GerenciadorDeProjetos.Domain.DTOs;
using Xunit;
using GerenciadorDeProjetos.Infrastructure.Interfaces;

namespace GerenciadorDeProjetos.Tests
{
    public class ProjetoRepositoryTests
    {
        private readonly ProjetoRepository _repository;

        public ProjetoRepositoryTests()
        {
            _repository = new ProjetoRepository();
        }

        [Fact]
        public void TesteAdicionarProjeto()
        {
            var projeto = new Projeto
            {
                Nome = "Projeto de Teste",
                Descricao = "Descrição do projeto de teste",
                DataInicio = DateTime.Now,
                DataTermino = DateTime.Now.AddMonths(1),
                Tarefas = new List<Tarefa>
                {
                    new Tarefa
                    {
                        Nome = "Tarefa 1",
                        Descricao = "Descrição da tarefa 1",
                        Prazo = DateTime.Now.AddDays(5),
                        StatusTarefa = "Em andamento",
                        UsuarioId = 2
                    },
                    new Tarefa
                    {
                        Nome = "Tarefa 2",
                        Descricao = "Descrição da tarefa 2",
                        Prazo = DateTime.Now.AddDays(10),
                        StatusTarefa = "Pendente",
                        UsuarioId = 2
                    }
                }
            };

            var resultado = _repository.Add(projeto);

            Assert.True(resultado, "O projeto deve ser adicionado com sucesso.");
        }

        [Fact]
        public void TesteObterProjetoPorId()
        {
            int idProjeto = 2;

            var resultado = _repository.GetById(idProjeto);

            Assert.NotNull(resultado);
            Assert.Equal(idProjeto, resultado.Id);
        }

        [Fact]
        public void TesteObterTodosProjetos()
        {
            var projetos = _repository.GetAll();

            Assert.NotNull(projetos);
            Assert.True(projetos.Any(), "Deve haver ao menos um projeto na base de dados.");
        }

        [Fact]
        public void TesteAtualizarProjeto()
        {
            var projeto = new Projeto
            {
                Id = 2,
                Nome = "Projeto Atualizado",
                Descricao = "Descrição atualizada",
                DataInicio = DateTime.Now.AddDays(2),
                DataTermino = DateTime.Now.AddMonths(2),
            };

            var resultado = _repository.Update(projeto);

            Assert.True(resultado, "A atualização do projeto deve retornar true.");
        }

        [Fact]
        public void TesteDeletarProjeto()
        {
            int idParaExcluir = 2;

            var resultado = _repository.Delete(idParaExcluir);

            Assert.True(resultado, "A exclusão do projeto deve retornar true.");
        }
    }
}
