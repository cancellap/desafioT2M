namespace GerenciadorDeProjetos.Domain.Exceptions
{
    public class UsuarioJaExisteException : Exception
    {
        public UsuarioJaExisteException(string username)
            : base($"O usuário com o nome de usuário '{username}' já existe.")
        {
        }
    }
}
