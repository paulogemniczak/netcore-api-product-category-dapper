using Dapper;
using Microsoft.Extensions.Configuration;
using Gemniczak.Domain.Entities;
using Gemniczak.Domain.Filters;
using Gemniczak.Domain.InterfaceRepositories;

namespace Gemniczak.Data.Repositories
{
    internal class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Product> Create(Product obj)
        {
            obj.ProductId = await Connection.QueryFirstAsync<int>("call sp_ProductCreate(@iProductName, @iProductCategoryId);",
                new
                {
                    iProductName = obj.ProductName,
                    iProductCategoryId = obj.ProductCategoryId
                }, transaction: Transaction);
            return obj;
        }

        public async Task<bool> Update(Product obj)
        {
            var affectedRows = await Connection.QueryFirstAsync<int>("call sp_ProductUpdate(@iProductId, @iProductName, @iProductCategoryId);",
                new
                {
                    iProductId = obj.ProductId,
                    iProductName = obj.ProductName,
                    iProductCategoryId = obj.ProductCategoryId
                }, transaction: Transaction);
            return affectedRows > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var affectedRows = await Connection.QueryFirstAsync<int>("call sp_ProductDelete(@iProductId);",
                new
                {
                    iProductId = id
                }, transaction: Transaction);
            return affectedRows > 0;
        }

        public async Task<Product> GetById(int id)
        {
            var result = await Connection.QueryFirstOrDefaultAsync<Product>("call sp_ProductGetById(@iProductId);",
                new
                {
                    iProductId = id
                }, transaction: Transaction);
            return result;
        }

        public async Task<IEnumerable<Product>> List(ProductFilter filter)
        {
            var result = await Connection.QueryAsync<Product>("call sp_ProductList(@iInputText, @iPageNumber, @iPageSize, @iOrderBy, @iIsAsc);",
                new
                {
                    iInputText = filter?.InputText,
                    iPageNumber = filter?.PageNumber,
                    iPageSize = filter?.PageSize,
                    iOrderBy = filter?.OrderBy,
                    iIsAsc = filter?.IsAsc
                }, transaction: Transaction);
            return result;
        }
    }
}
