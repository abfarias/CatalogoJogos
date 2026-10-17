namespace CatalogoJogos.Interfaces
{
    public interface IRepository<T> where T : class
    {
        List<T> ListarTodos();
        void Adicionar(T entidade);
    }
}