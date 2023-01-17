using Dapper;
using Microsoft.Extensions.Configuration;
using Gemniczak.Domain.Entities;
using Gemniczak.Domain.Filters;
using Gemniczak.Domain.InterfaceRepositories;

namespace Gemniczak.Data.Repositories
{
    internal class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Category> Create(Category obj)
        {
            obj.CategoryId = await Connection.QueryFirstAsync<int>("call sp_CategoryCreate(@iCategoryName);",
                new
                {
                    iCategoryName = obj.CategoryName
                }, transaction: Transaction);
            return obj;
        }

        public async Task<bool> Update(Category obj)
        {
            var affectedRows = await Connection.QueryFirstAsync<int>("call sp_CategoryUpdate(@iCategoryId, @iCategoryName);",
                new
                {
                    iCategoryId = obj.CategoryId,
                    iCategoryName = obj.CategoryName
                }, transaction: Transaction);
            return affectedRows > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var affectedRows = await Connection.QueryFirstAsync<int>("call sp_CategoryDelete(@iCategoryId);",
                new
                {
                    iCategoryId = id
                }, transaction: Transaction);
            return affectedRows > 0;
        }

        public async Task<Category> GetById(int id)
        {
            var result = await Connection.QueryFirstOrDefaultAsync<Category>("call sp_CategoryGetById(@iCategoryId);",
                new
                {
                    iCategoryId = id
                }, transaction: Transaction);
            return result;
        }

        public async Task<IEnumerable<Category>> List(CategoryFilter filter)
        {
            var result = await Connection.QueryAsync<Category>("call sp_CategoryList(@iInputText, @iPageNumber, @iPageSize, @iOrderBy, @iIsAsc);", 
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
