using CatalogoJogos.Interfaces;
using CatalogoJogos.Models;

namespace CatalogoJogos.Services
{
    public class AdminCatalogoService
    {
        private const string MensagemJogoNaoEncontrado = "Jogo não encontrado.";

        private readonly IJogoRepository _jogoRepository;
        private readonly IAdministradorRepository _administradorRepository;
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

        public AdminCatalogoService(
            IJogoRepository jogoRepository,
            IAdministradorRepository administradorRepository,
            IUnidadeDeTrabalho unidadeDeTrabalho)
        {
            _jogoRepository = jogoRepository;
            _administradorRepository = administradorRepository;
            _unidadeDeTrabalho = unidadeDeTrabalho;
        }

        public Task<List<Jogo>> ListarTodosAsync()
        {
            return _jogoRepository.BuscarAsync(termo: null, categoria: null, somenteAtivos: false);
        }

        public Task<Jogo?> ObterPorIdAsync(int idJogo)
        {
            return _jogoRepository.ObterPorIdAsync(idJogo);
        }

        public async Task<ResultadoOperacao<Jogo>> CadastrarAsync(Jogo jogo, int idAdmin)
        {
            if (await _administradorRepository.ObterPorIdAsync(idAdmin) is null)
                return ResultadoOperacao<Jogo>.Falha("Administrador não encontrado.");

            // Cria uma nova instância para que Id, situação e responsável não venham do formulário.
            var novoJogo = new Jogo
            {
                DataCadastro = DateTime.Now,
                Ativo = true,
                IdAdminCadastro = idAdmin
            };
            novoJogo.AtualizarDados(jogo.Titulo, jogo.Descricao, jogo.Categoria, jogo.Preco, jogo.CaminhoFotoCapa);

            await _jogoRepository.AdicionarAsync(novoJogo);
            await _unidadeDeTrabalho.SalvarAlteracoesAsync();

            return ResultadoOperacao<Jogo>.Ok(novoJogo, $"Jogo \"{novoJogo.Titulo}\" adicionado.");
        }

        public async Task<ResultadoOperacao<Jogo>> EditarAsync(int idJogo, Jogo dados)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(idJogo);
            if (jogo is null)
                return ResultadoOperacao<Jogo>.Falha(MensagemJogoNaoEncontrado);

            jogo.AtualizarDados(dados.Titulo, dados.Descricao, dados.Categoria, dados.Preco, dados.CaminhoFotoCapa);
            await _unidadeDeTrabalho.SalvarAlteracoesAsync();

            return ResultadoOperacao<Jogo>.Ok(jogo, $"Jogo \"{jogo.Titulo}\" atualizado.");
        }

        public async Task<ResultadoOperacao<Jogo>> AlterarSituacaoAsync(int idJogo)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(idJogo);
            if (jogo is null)
                return ResultadoOperacao<Jogo>.Falha(MensagemJogoNaoEncontrado);

            if (jogo.Ativo)
                jogo.Desativar();
            else
                jogo.Reativar();

            await _unidadeDeTrabalho.SalvarAlteracoesAsync();

            var situacao = jogo.Ativo ? "reativado" : "desativado";
            return ResultadoOperacao<Jogo>.Ok(jogo, $"Jogo \"{jogo.Titulo}\" {situacao}.");
        }
    }
}
