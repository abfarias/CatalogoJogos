using CatalogoJogos.Interfaces;
using CatalogoJogos.Models;
using System.Globalization;

namespace CatalogoJogos.Mappers
{
    public class JogoCsvMapper : ICsvMapper<Jogo>
    {
        public string CaminhoArquivo => "Data/jogos.csv";

        public string Cabecalho => "IdJogo;Titulo;Descricao;Categoria;Preco;CaminhoFotoCapa";

        public void AtribuirNovoId(List<Jogo> entidadesExistentes, Jogo novaEntidade)
        {
            novaEntidade.IdJogo = entidadesExistentes.Any() ? entidadesExistentes.Max(j => j.IdJogo) + 1 : 1;
        }

        public Jogo MapearParaEntidade(string[] linha)
        {
            return new Jogo
            {
                IdJogo = int.Parse(linha[0]),
                Titulo = linha[1],
                Descricao = linha[2],
                Categoria = linha[3],
                Preco = decimal.Parse(linha[4], CultureInfo.InvariantCulture),
                CaminhoFotoCapa = linha[5]
            };
        }

        public string MapearParaLinha(Jogo entidade)
        {
            return $"{entidade.IdJogo};{entidade.Titulo};{entidade.Descricao};{entidade.Categoria};{entidade.Preco.ToString(CultureInfo.InvariantCulture)};{entidade.CaminhoFotoCapa}";
        }
    }
}
