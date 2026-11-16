namespace CatalogoJogos.Services
{
    public class ResultadoOperacao<T>
    {
        public bool Sucesso { get; }
        public string Mensagem { get; }
        public T? Valor { get; }

        private ResultadoOperacao(bool sucesso, string mensagem, T? valor)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
            Valor = valor;
        }

        public static ResultadoOperacao<T> Ok(T valor, string mensagem = "") => new(true, mensagem, valor);

        public static ResultadoOperacao<T> Falha(string mensagem) => new(false, mensagem, default);
    }
}
