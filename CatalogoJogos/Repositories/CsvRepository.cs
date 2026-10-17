using CatalogoJogos.Interfaces;
using System.Text;

namespace CatalogoJogos.Repositories
{
    public class CsvRepository<T> : IRepository<T> where T : class
    {
        private readonly ICsvMapper<T> _mapper;

        public CsvRepository(ICsvMapper<T> mapper)
        {
            _mapper = mapper;
            GarantirArquivoEDiretorio();
        }

        private void GarantirArquivoEDiretorio()
        {
            string caminhoDoArquivo = _mapper.CaminhoArquivo;
            
            var directory = Path.GetDirectoryName(caminhoDoArquivo);
            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (!File.Exists(caminhoDoArquivo))
                File.WriteAllText(caminhoDoArquivo, _mapper.Cabecalho + Environment.NewLine, Encoding.UTF8);
        }

        public List<T> ListarTodos()
        {
            var linhas = File.ReadAllLines(_mapper.CaminhoArquivo).Skip(1);
            var entidades = new List<T>();

            foreach (var linha in linhas)
            {
                if (string.IsNullOrWhiteSpace(linha)) continue;
                entidades.Add(_mapper.MapearParaEntidade(linha.Split(';')));
            }
            return entidades;
        }

        public void Adicionar(T entidade)
        {
            var entidades = ListarTodos();
            _mapper.AtribuirNovoId(entidades, entidade);

            var linhaCsv = _mapper.MapearParaLinha(entidade);
            File.AppendAllText(_mapper.CaminhoArquivo, linhaCsv + Environment.NewLine, Encoding.UTF8);
        }
    }
}
