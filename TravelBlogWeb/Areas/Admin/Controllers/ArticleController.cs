using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using TravelBlog.Entity.Entities;
using TravelBlog.Entity.ViewModels.Articles;
using TravelBlog.Service.Extensions;
using TravelBlog.Service.Services.Abstractions;
using TravelBlog.Web.Consts;
using TravelBlog.Web.ResultMessages;

namespace TravelBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly IArticleService articleService;
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        private readonly IValidator<Article> validator;
        private readonly IToastNotification toast;

        public ArticleController(IArticleService articleService, ICategoryService categoryService, IMapper mapper, IValidator<Article> validator, IToastNotification toast)
        {
            this.articleService = articleService;
            this.categoryService = categoryService;
            this.mapper = mapper;
            this.validator = validator;
            this.toast = toast;
        }
        [HttpGet]
        [Authorize(Roles =$"{RoleConsts.Superadmin}, {RoleConsts.Admin},{RoleConsts.User}")]
        public async Task<IActionResult> Index()
        {
            var articles = await articleService.GetAllArticlesWithCategoryNonDeletedAsync();
            return View(articles);
        }
        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> DeletedArticles()
        {
            var articles = await articleService.GetAllArticlesWithCategoryDeletedAsync();
            return View(articles);
        }
        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Add()
        {
            var categories = await categoryService.GetAllCategoriesNonDeleted();
            return View(new ArticleAddViewModel { Categories = categories });
        }
        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Add(ArticleAddViewModel articleAddVm)
        {
            var map = mapper.Map<Article>(articleAddVm);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await articleService.CreateArticleAsync(articleAddVm);
                toast.AddSuccessToastMessage(Messages.Article.Add(articleAddVm.Title), new ToastrOptions { Title = "Başarılı" });
                return RedirectToAction("Index", "Article", new { Area = "Admin" });
            }
            else
            {
                result.AddToModelState(this.ModelState);
            }
            var categories = await categoryService.GetAllCategoriesNonDeleted();
            return View(new ArticleAddViewModel { Categories = categories });
        }
        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Update(Guid articleId)
        {
            var article = await articleService.GetArticleWithCategoryNonDeletedAsync(articleId);
            var categories = await categoryService.GetAllCategoriesNonDeleted();
            var articleUpdateVm = mapper.Map<ArticleUpdateViewModel>(article);
            articleUpdateVm.Categories = categories;

            return View(articleUpdateVm);
        }
        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Update(ArticleUpdateViewModel articleUpdateVm)//[FromForm]
        {
            var map = mapper.Map<Article>(articleUpdateVm);
            var result = await validator.ValidateAsync(map);
            if (result.IsValid)
            {
                var title = await articleService.UpdateArticleAsync(articleUpdateVm);
                toast.AddSuccessToastMessage(Messages.Article.Update(title), new ToastrOptions { Title = "Başarılı" });
                return RedirectToAction("Index", "Article", new { Area = "Admin" });
            }
            else
            {
                result.AddToModelState(this.ModelState);
            }
            var categories = await categoryService.GetAllCategoriesNonDeleted();
            articleUpdateVm.Categories = categories;

            return View(articleUpdateVm);
        }
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Delete(Guid articleId)
        {
            var title = await articleService.SafeDeleteArticleAsync(articleId);
            toast.AddSuccessToastMessage(Messages.Article.Delete(title), new ToastrOptions { Title = "Başarılı" });

            return RedirectToAction("Index", "Article", new { Area = "Admin" });
        }
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> UndoDelete(Guid articleId)
        {
            var title = await articleService.UndoDeleteArticleAsync(articleId);
            toast.AddSuccessToastMessage(Messages.Article.UndoDelete(title), new ToastrOptions { Title = "Başarılı" });

            return RedirectToAction("Index", "Article", new { Area = "Admin" });
        }
    }
}
