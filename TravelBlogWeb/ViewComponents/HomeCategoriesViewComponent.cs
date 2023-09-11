using Microsoft.AspNetCore.Mvc;
using TravelBlog.Service.Services.Abstractions;

namespace TravelBlog.Web.ViewComponents
{
    public class HomeCategoriesViewComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;

        public HomeCategoriesViewComponent(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await categoryService.GetAllCategoriesNonDeletedTake();
            return View(categories);
        }
    }
}
