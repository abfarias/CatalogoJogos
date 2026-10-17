namespace CatalogoJogos.Interfaces
{
    public interface IRepository<T> where T : class
    {
        List<T> ListasTodos();
        void Adicionar(T entidade);
    }
}