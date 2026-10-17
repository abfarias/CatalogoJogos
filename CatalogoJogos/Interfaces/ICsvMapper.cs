namespace CatalogoJogos.Interfaces
{
    public interface ICsvMapper<T> where T : class
    {
        string CaminhoArquivo { get; }
        string Cabecalho { get; }

        T MapearParaEntidade(string[] linha);
        string MapearParaLinha(T entidade);
        void AtribuirNovoId(List<T> entidadesExistentes, T novaEntidade);
    }
}
