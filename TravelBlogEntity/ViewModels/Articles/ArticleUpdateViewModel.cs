using Microsoft.AspNetCore.Http;
using TravelBlog.Entity.Entities;
using TravelBlog.Entity.ViewModels.Categories;

namespace TravelBlog.Entity.ViewModels.Articles
{
    public class ArticleUpdateViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Image Image { get; set; }
        public IFormFile? Picture { get; set; }
        public Guid CategoryId { get; set; }
        public IList<CategoryViewModel> Categories { get; set; }
    }
}
