using Gemniczak.Domain.Entities;
using Gemniczak.Domain.Filters;

namespace Gemniczak.Domain.InterfaceRepositories
{
    public interface ICategoryRepository : IRepository<Category, CategoryFilter>
    {
        // add new methods here if that is necessary
    }
}
