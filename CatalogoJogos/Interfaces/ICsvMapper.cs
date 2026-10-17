namespace CatalogoJogos.Interfaces
{
    public interface ICsvMapper<T> where T : class
    {
        string CaminhoArquivo { get; }
        string Cabecalho { get; }

        T MapearParaEntidade(string[] linhas);
        string MapearParaLinha(T entidade);
        void AtribuirNovoId(List<T> entidadesExistentes, T novaEntidade);
    }
}
