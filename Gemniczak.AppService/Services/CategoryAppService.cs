using Mapster;
using Gemniczak.AppService.Dtos;
using Gemniczak.AppService.Filters;
using Gemniczak.AppService.Interfaces;
using Gemniczak.Domain;
using Gemniczak.Domain.Entities;
using Gemniczak.Domain.Filters;
using Gemniczak.Domain.InterfaceRepositories;

namespace Methods.AppService.Services
{
    internal class CategoryAppService : ICategoryAppService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _uow;

        public CategoryAppService(
            IUnitOfWork uow,
            ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _uow = uow;
        }

        public async Task<CategoryDto> Create(CategoryDto categoryDto)
        {
            CategoryDto result;
            using (var uowTransaction = _uow.Begin(_categoryRepository))
            {
                try
                {
                    Category category = await _categoryRepository.Create(categoryDto.Adapt<Category>());
                    result = category.Adapt<CategoryDto>();
                    uowTransaction.Commit();
                }
                catch
                {
                    uowTransaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        public async Task<bool> Delete(int id)
        {
            return await _categoryRepository.Delete(id);
        }

        public async Task<CategoryDto> GetById(int id)
        {
            Category category = await _categoryRepository.GetById(id);
            return category.Adapt<CategoryDto>();
        }

        public async Task<IEnumerable<CategoryDto>> List(CategoryFilterDto filterDto)
        {
            IEnumerable<Category> list = await _categoryRepository.List(filterDto.Adapt<CategoryFilter>());
            return list.Adapt<IEnumerable<CategoryDto>>();
        }

        public async Task<bool> Update(CategoryDto categoryDto)
        {
            return await _categoryRepository.Update(categoryDto.Adapt<Category>());
        }
    }
}
