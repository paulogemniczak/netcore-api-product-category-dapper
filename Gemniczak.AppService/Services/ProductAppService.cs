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
    internal class ProductAppService : IProductAppService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _uow;

        public ProductAppService(
            IUnitOfWork uow,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _uow = uow;
            _categoryRepository = categoryRepository;
        }

        public async Task<ProductDto> Create(ProductDto productDto)
        {
            ProductDto result;
            using (var uowTransaction = _uow.Begin(_productRepository))
            {
                try
                {
                    productDto.ProductCategoryId = productDto.ProductCategory.CategoryId;
                    Product product = await _productRepository.Create(productDto.Adapt<Product>());
                    result = product.Adapt<ProductDto>();
                    result.ProductCategory = productDto.ProductCategory;
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
            return await _productRepository.Delete(id);
        }

        public async Task<ProductDto> GetById(int id)
        {
            Product product = await _productRepository.GetById(id);
            Category category = await _categoryRepository.GetById(product.ProductCategoryId);

            ProductDto productDto = product.Adapt<ProductDto>();
            productDto.ProductCategory = category.Adapt<CategoryDto>();

            return product.Adapt<ProductDto>();
        }

        public async Task<IEnumerable<ProductDto>> List(ProductFilterDto filterDto)
        {
            IEnumerable<Product> list = await _productRepository.List(filterDto.Adapt<ProductFilter>());
            IEnumerable<ProductDto> listDto = list.Adapt<IEnumerable<ProductDto>>();

            foreach(ProductDto productDto in listDto)
            {
                Category category = await _categoryRepository.GetById(productDto.ProductCategoryId ?? default);
                productDto.ProductCategory = category.Adapt<CategoryDto>();
            }

            return listDto;
        }

        public async Task<bool> Update(ProductDto productDto)
        {
            return await _productRepository.Update(productDto.Adapt<Product>());
        }
    }
}
