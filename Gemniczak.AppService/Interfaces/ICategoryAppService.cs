using Gemniczak.AppService.Dtos;
using Gemniczak.AppService.Filters;

namespace Gemniczak.AppService.Interfaces
{
    public interface ICategoryAppService
    {
        Task<CategoryDto> Create(CategoryDto obj);
        Task<bool> Update(CategoryDto obj);
        Task<bool> Delete(int id);
        Task<CategoryDto> GetById(int id);
        Task<IEnumerable<CategoryDto>> List(CategoryFilterDto filter);
    }
}
