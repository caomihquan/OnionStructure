using Onion.Cache.Cache;
using Onion.Datas.Abstract;
using Onion.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Services.CategoryService
{

    public class CategoryService : ICategoryService
    {
        IResponsitory<Category> _categoryResponsitory;
        IDapperHelper _dapperHelper;
        IDistributedCacheService _cache;

        public CategoryService(IResponsitory<Category> categoryResponsitory, IDapperHelper dapperHelper, IDistributedCacheService cache)
        {
            _categoryResponsitory = categoryResponsitory;
            _dapperHelper = dapperHelper;
            _cache = cache;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            string sql = "select top(1) * from Categories";
            var resultCache = await _cache.Get<IEnumerable<Category>>("categories");
            if(resultCache == null)
            {
                resultCache = await _dapperHelper.ExcuteSqlReturnList<Category>(sql);

            }
            await _cache.Set<IEnumerable<Category>>("categories",resultCache);
            return resultCache;
        }
    }
}
