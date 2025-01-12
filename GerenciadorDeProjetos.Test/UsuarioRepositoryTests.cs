using System.Linq;
using Dapper;
using GerenciadorDeProjetos.Domain.Entities;
using GerenciadorDeProjetos.Domain.Interface;
using Xunit;

namespace GerenciadorDeProjetos.Tests
{
    public class UsuarioRepositoryTests
    {
        private readonly UsuarioRepository _repository;

        public UsuarioRepositoryTests()
        {
            _repository = new UsuarioRepository();
        }

        [Fact]
        public void TesteAdicionarUsuario()
        {
            var usuario = new Usuario { Nome = "Teste", Cargo = "Desenvolvedor" };

            var resultado = _repository.Add(usuario);

            Assert.NotNull(resultado);
            Assert.True(resultado.Id > 0, "O ID do usuário deve ser maior que zero.");
        }

        [Fact]
        public void TesteObterTodosUsuarios()
        {
            var usuarios = _repository.GetAll();

            Assert.NotNull(usuarios);
            Assert.True(usuarios.Any(), "Deve haver ao menos um usuário na base de dados.");
        }

        [Fact]
        public void TesteAtualizarUsuario()
        {
            var usuario = new Usuario { Id = 1, Nome = "Nome Atualizado", Cargo = "Cargo Atualizado" };

            var resultado = _repository.Update(usuario);

            Assert.True(resultado, "A atualização do usuário deve retornar true.");
        }

        [Fact]
        public void TesteDeletarUsuario()
        {
            int idParaExcluir = 1;

            var resultado = _repository.Delete(idParaExcluir);

            Assert.True(resultado, "A exclusão do usuário deve retornar true.");
        }
    }
}
