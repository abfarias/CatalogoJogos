namespace CatalogoJogos.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> ListarTodosAsync();
        Task<T?> ObterPorIdAsync(int id);
        Task AdicionarAsync(T entidade);
    }
}
