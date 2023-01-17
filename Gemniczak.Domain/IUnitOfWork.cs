namespace Gemniczak.Domain
{
  public interface IUnitOfWork
  {
    IUnitOfWorkTransaction Begin(params IRepository[] repositories);
  }
}
