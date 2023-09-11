using TravelBlog.Entity.Entities;
using TravelBlog.Entity.ViewModels.Categories;

namespace TravelBlog.Entity.ViewModels.Articles
{
    public class ArticleViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public CategoryViewModel Category { get; set; }
        public Image Image { get; set; }
        public AppUser User { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int ViewCount { get; set; }
    }
}
