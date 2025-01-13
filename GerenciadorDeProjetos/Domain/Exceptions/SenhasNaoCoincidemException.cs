namespace GerenciadorDeProjetos.Domain.Exceptions
{
    public class SenhasNaoCoincidemException : Exception
    {
        public SenhasNaoCoincidemException()
            : base($"As senhas não coincidem")
        {
        }
    }
}
