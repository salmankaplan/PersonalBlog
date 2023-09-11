using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Data;
using TravelBlog.Entity.Entities;
using TravelBlog.Entity.ViewModels.Categories;
using TravelBlog.Service.Extensions;
using TravelBlog.Service.Services.Abstractions;
using TravelBlog.Service.Services.Concretes;
using TravelBlog.Web.Consts;
using TravelBlog.Web.ResultMessages;

namespace TravelBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IValidator<Category> validator;
        private readonly IMapper mapper;
        private readonly IToastNotification toast;

        public CategoryController(ICategoryService categoryService, IValidator<Category> validator, IMapper mapper, IToastNotification toast)
        {
            this.categoryService = categoryService;
            this.validator = validator;
            this.mapper = mapper;
            this.toast = toast;
        }
        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin},{RoleConsts.User}")]
        public async Task<IActionResult> Index()
        {
            var categories = await categoryService.GetAllCategoriesNonDeleted();
            return View(categories);
        }
        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> DeletedCategories()
        {
            var categories = await categoryService.GetAllCategoriesDeleted();
            return View(categories);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Add(CategoryAddViewModel categoryAddVm)
        {
            var map = mapper.Map<Category>(categoryAddVm);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await categoryService.CreateCategoryAsync(categoryAddVm);
                toast.AddSuccessToastMessage(Messages.Category.Add(categoryAddVm.Name), new ToastrOptions { Title = "Başarılı" });
                return RedirectToAction("Index", "Category", new { Area = "Admin" });
            }

            result.AddToModelState(this.ModelState);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddWithAjax([FromBody] CategoryAddViewModel categoryAddVm)
        {
            var map = mapper.Map<Category>(categoryAddVm);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await categoryService.CreateCategoryAsync(categoryAddVm);
                toast.AddSuccessToastMessage(Messages.Category.Add(categoryAddVm.Name), new ToastrOptions { Title = "Başarılı" });
                return Json(Messages.Category.Add(categoryAddVm.Name));
            }
            else
            {
                toast.AddErrorToastMessage(result.Errors.First().ErrorMessage, new ToastrOptions { Title = "İşlem Başarısız" });
                return Json(result.Errors.First().ErrorMessage);
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Update(Guid categoryId)
        {
            var category = await categoryService.GetCategoryByGuid(categoryId);
            var map = mapper.Map<Category, CategoryUpdateViewModel>(category);
            return View(map);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Update(CategoryUpdateViewModel categoryUpdateVm)
        {
            var map = mapper.Map<Category>(categoryUpdateVm);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                var name = await categoryService.UpdateCategoryAsync(categoryUpdateVm);
                toast.AddSuccessToastMessage(Messages.Category.Update(name), new ToastrOptions { Title = "Başarılı" });
                return RedirectToAction("Index", "Category", new { Area = "Admin" });
            }
            result.AddToModelState(this.ModelState);
            return View();
        }
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Delete(Guid categoryId)
        {
            var name = await categoryService.SafeDeleteArticleAsync(categoryId);
            toast.AddSuccessToastMessage(Messages.Category.Delete(name), new ToastrOptions { Title = "Başarılı" });

            return RedirectToAction("Index", "Category", new { Area = "Admin" });
        }
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> UndoDelete(Guid categoryId)
        {
            var name = await categoryService.UndoDeleteArticleAsync(categoryId);
            toast.AddSuccessToastMessage(Messages.Category.UndoDelete(name), new ToastrOptions { Title = "Başarılı" });

            return RedirectToAction("Index", "Category", new { Area = "Admin" });
        }
    }
}
