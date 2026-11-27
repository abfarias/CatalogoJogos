using CatalogoJogos.Models;

namespace CatalogoJogos.Services
{
    // Dados da página de detalhes: o jogo, a nota média, as avaliações e se o jogador logado possui o jogo.
    public class DetalhesJogo
    {
        public required Jogo Jogo { get; init; }
        public double? NotaMedia { get; init; }
        public List<Avaliacao> Avaliacoes { get; init; } = new();
        public bool Possui { get; init; }
    }
}
