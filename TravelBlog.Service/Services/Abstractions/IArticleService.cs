using TravelBlog.Entity.ViewModels.Articles;

namespace TravelBlog.Service.Services.Abstractions
{
    public interface IArticleService
    {
        Task<List<ArticleViewModel>> GetAllArticlesWithCategoryNonDeletedAsync();
        Task<List<ArticleViewModel>> GetAllArticlesWithCategoryDeletedAsync();
        Task<ArticleViewModel> GetArticleWithCategoryNonDeletedAsync(Guid articleId);
        Task CreateArticleAsync(ArticleAddViewModel articleAddDto);
        Task<string> UpdateArticleAsync(ArticleUpdateViewModel articleUpdateDto);
        Task<string> SafeDeleteArticleAsync(Guid articleId);
        Task<string> UndoDeleteArticleAsync(Guid articleId);
        Task<ArticleListViewModel> GetAllByPagingAsync(Guid? categoryId, int currentPage = 1, int pageSize = 3, bool isAscending = false);
        Task<ArticleListViewModel> SearchAsync(string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false);

    }
}
