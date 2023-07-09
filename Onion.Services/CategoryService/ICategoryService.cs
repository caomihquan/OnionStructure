using Onion.Domains.Entities;

namespace Onion.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategories();
    }
}