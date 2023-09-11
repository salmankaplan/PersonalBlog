using Microsoft.AspNetCore.Http;
using TravelBlog.Entity.ViewModels.Categories;

namespace TravelBlog.Entity.ViewModels.Articles
{
    public class ArticleAddViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid CategoryId { get; set; }
        public IFormFile Picture { get; set; }
        public IList<CategoryViewModel> Categories { get; set; }
    }
}
