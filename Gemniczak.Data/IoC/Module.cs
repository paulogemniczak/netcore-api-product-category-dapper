using Gemniczak.Data.Repositories;
using Gemniczak.Domain;
using Gemniczak.Domain.InterfaceRepositories;

namespace Gemniczak.Data.IoC
{
  public static class Module
  {
    public static Dictionary<Type, Type> GetTypes()
    {
      Dictionary<Type, Type> dictionary = new()
      {
        {typeof(IUnitOfWork), typeof(UnitOfWork)},
        {typeof(ICategoryRepository), typeof(CategoryRepository)},
        {typeof(IProductRepository), typeof(ProductRepository)},
      };

      return dictionary;
    }
  }
}
