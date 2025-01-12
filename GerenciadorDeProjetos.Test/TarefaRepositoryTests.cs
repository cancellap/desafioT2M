using System.Linq;
using GerenciadorDeProjetos.Domain.Entities;
using GerenciadorDeProjetos.Domain.Interface;
using GerenciadorDeProjetos.Domain.DTOs;
using Xunit;

namespace GerenciadorDeProjetos.Tests
{
    public class TarefaRepositoryTests
    {
        private readonly TarefaRepository _repository;

        public TarefaRepositoryTests()
        {
            _repository = new TarefaRepository();
        }

        [Fact]
        public void TesteAdicionarTarefa()
        {
            var tarefaInsertDto = new TarefaInsertDto
            {
                Nome = "Tarefa de Teste",
                Descricao = "Descrição da tarefa de teste",
                Prazo = DateTime.Now.AddDays(5),
                Status = "Em andamento",
                UsuarioId = 2,
                ProjetoId = 3
            };

            var resultado = _repository.Add(tarefaInsertDto);

            Assert.NotNull(resultado);
            Assert.True(resultado.Id > 0, "O ID da tarefa deve ser maior que zero.");
        }

        [Fact]
        public void TesteObterTarefaPorId()
        {
            int idTarefa = 4; 

            var resultado = _repository.GetById(idTarefa);

            Assert.NotNull(resultado);
            Assert.Equal(idTarefa, resultado.Id);
        }

        [Fact]
        public void TesteObterTodasTarefas()
        {
            var tarefas = _repository.GetAll();

            Assert.NotNull(tarefas);
            Assert.True(tarefas.Any(), "Deve haver ao menos uma tarefa na base de dados.");
        }

        [Fact]
        public void TesteAtualizarTarefa()
        {
            var tarefa = new Tarefa
            {
                Id = 6, 
                Nome = "Tarefa Atualizada",
                Descricao = "Descrição atualizada",
                Prazo = DateTime.Now.AddDays(7),
                Status = "Concluída",
                UsuarioId = 2,
                ProjetoId = 3
            };

            var resultado = _repository.Update(tarefa);

            Assert.True(resultado, "A atualização da tarefa deve retornar true.");
        }

        [Fact]
        public void TesteDeletarTarefa()
        {
            int idParaExcluir = 5;

            var resultado = _repository.Delete(idParaExcluir);

            Assert.True(resultado, "A exclusão da tarefa deve retornar true.");
        }
    }
}
