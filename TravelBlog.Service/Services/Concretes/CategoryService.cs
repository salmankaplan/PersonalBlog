using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TravelBlog.Data.UnitOfWorks;
using TravelBlog.Entity.Entities;
using TravelBlog.Entity.ViewModels.Categories;
using TravelBlog.Service.Extensions;
using TravelBlog.Service.Services.Abstractions;

namespace TravelBlog.Service.Services.Concretes
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal _user;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
        }
        public async Task<List<CategoryViewModel>> GetAllCategoriesNonDeleted()
        {
            var categories = await unitOfWork.GetRepository<Category>().GetAllAsync(x => !x.IsDeleted);
            var map = mapper.Map<List<CategoryViewModel>>(categories);
            return map;
        }
        public async Task CreateCategoryAsync(CategoryAddViewModel categoryAddVm)
        {
            var userId = _user.GetLoggedInUserId();
            var userEmail = _user.GetLoggedInEmail();

            Category category = new(categoryAddVm.Name, userEmail);
            await unitOfWork.GetRepository<Category>().AddAsync(category);
            await unitOfWork.SaveAsync();
        }
        public async Task<Category> GetCategoryByGuid(Guid id)
        {
            var category = await unitOfWork.GetRepository<Category>().GetByGuidAsync(id);

            return category;
        }
        public async Task<string> UpdateCategoryAsync(CategoryUpdateViewModel categoryUpdateVm)
        {
            var category = await unitOfWork.GetRepository<Category>().GetAsync(x => !x.IsDeleted && x.Id == categoryUpdateVm.Id);
            var userEmail = _user.GetLoggedInEmail();

            category.Name = categoryUpdateVm.Name;
            category.ModifiedBy = userEmail;
            category.ModifiedDate= DateTime.Now;

            await unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await unitOfWork.SaveAsync();

            return category.Name;
        }

        public async Task<string> SafeDeleteArticleAsync(Guid categoryId)
        {
            var category = await unitOfWork.GetRepository<Category>().GetByGuidAsync(categoryId);
            var userEmail = _user.GetLoggedInEmail();

            category.IsDeleted = true;
            category.DeleteDate = DateTime.Now;
            category.DeletedBy = userEmail;

            await unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await unitOfWork.SaveAsync();

            return category.Name;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoriesDeleted()
        {
            var categories = await unitOfWork.GetRepository<Category>().GetAllAsync(x => x.IsDeleted);
            var map = mapper.Map<List<CategoryViewModel>>(categories);

            return map;
        }

        public async Task<string> UndoDeleteArticleAsync(Guid categoryId)
        {
            var category = await unitOfWork.GetRepository<Category>().GetByGuidAsync(categoryId);

            category.IsDeleted = false;
            category.DeleteDate = null;
            category.DeletedBy = null;

            await unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await unitOfWork.SaveAsync();

            return category.Name;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoriesNonDeletedTake()
        {
            var categories = await unitOfWork.GetRepository<Category>().GetAllAsync(x => !x.IsDeleted);
            var map = mapper.Map<List<CategoryViewModel>>(categories);

            return map.Take(24).ToList();
        }
    }
}
