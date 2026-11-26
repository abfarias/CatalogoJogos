using CatalogoJogos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogoJogos.Services
{
    public class CarteiraService
    {
        public const decimal RecargaMinima = 1m;
        public const decimal RecargaMaxima = 1000m;

        private readonly IJogadorRepository _jogadorRepository;
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

        public CarteiraService(IJogadorRepository jogadorRepository, IUnidadeDeTrabalho unidadeDeTrabalho)
        {
            _jogadorRepository = jogadorRepository;
            _unidadeDeTrabalho = unidadeDeTrabalho;
        }

        public async Task<decimal> ObterSaldoAsync(int idJogador)
        {
            var jogador = await _jogadorRepository.ObterPorIdAsync(idJogador);
            return jogador?.SaldoCarteira ?? 0m;
        }

        public async Task<ResultadoOperacao<decimal>> AdicionarSaldoAsync(int idJogador, decimal valor)
        {
            if (valor < RecargaMinima || valor > RecargaMaxima)
                return ResultadoOperacao<decimal>.Falha("A recarga deve ser de R$ 1,00 a R$ 1.000,00.");

            var jogador = await _jogadorRepository.ObterPorIdAsync(idJogador);
            if (jogador is null)
                return ResultadoOperacao<decimal>.Falha("Jogador não encontrado.");

            jogador.Creditar(valor);

            try
            {
                await _unidadeDeTrabalho.SalvarAlteracoesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return ResultadoOperacao<decimal>.Falha("O saldo foi alterado por outra operação. Tente novamente.");
            }

            return ResultadoOperacao<decimal>.Ok(jogador.SaldoCarteira, "Saldo adicionado.");
        }
    }
}
