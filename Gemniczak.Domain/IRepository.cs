namespace Gemniczak.Domain
{
  public interface IRepository
  {
  }

  public interface IRepository<T, in TFilter> : IRepository
  {
    Task<T> Create(T obj);
    Task<bool> Update(T obj);
    Task<bool> Delete(int id);
    Task<T> GetById(int id);
    Task<IEnumerable<T>> List(TFilter filter);
  }
}
