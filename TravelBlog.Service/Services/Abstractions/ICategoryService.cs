using TravelBlog.Entity.Entities;
using TravelBlog.Entity.ViewModels.Categories;

namespace TravelBlog.Service.Services.Abstractions
{
    public interface ICategoryService
    {
        Task<List<CategoryViewModel>> GetAllCategoriesNonDeleted();
        Task<List<CategoryViewModel>> GetAllCategoriesDeleted();
        Task CreateCategoryAsync(CategoryAddViewModel categoryAddVm);
        Task<Category> GetCategoryByGuid(Guid id);
        Task<string> UpdateCategoryAsync(CategoryUpdateViewModel categoryUpdateVm);
        Task<string> SafeDeleteArticleAsync(Guid categoryId);
        Task<string> UndoDeleteArticleAsync(Guid categoryId);
        Task<List<CategoryViewModel>> GetAllCategoriesNonDeletedTake();
    }
}
