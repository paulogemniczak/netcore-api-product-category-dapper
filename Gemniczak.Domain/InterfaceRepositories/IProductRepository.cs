using Gemniczak.Domain.Entities;
using Gemniczak.Domain.Filters;

namespace Gemniczak.Domain.InterfaceRepositories
{
    public interface IProductRepository : IRepository<Product, ProductFilter>
    {
        // add new methods here if that is necessary
    }
}
