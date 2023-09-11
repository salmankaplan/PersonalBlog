using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TravelBlog.Data.UnitOfWorks;
using TravelBlog.Entity.Entities;
using TravelBlog.Entity.Enums;
using TravelBlog.Entity.ViewModels.Articles;
using TravelBlog.Service.Extensions;
using TravelBlog.Service.Helpers.Images;
using TravelBlog.Service.Services.Abstractions;

namespace TravelBlog.Service.Services.Concretes
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IImageHelper imageHelper;
        private readonly ClaimsPrincipal _user;
        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IImageHelper imageHelper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
            this.imageHelper = imageHelper;
        }

        public async Task CreateArticleAsync(ArticleAddViewModel articleAddVm)
        {
            //var userId = Guid.Parse("391D592B-EED1-4E72-94AB-10B341D0739C");
            var userId = _user.GetLoggedInUserId();
            var userEmail = _user.GetLoggedInEmail();

            var imageUpload = await imageHelper.Upload(articleAddVm.Title, articleAddVm.Picture, ImageType.Post);
            Image image = new(imageUpload.FullName, articleAddVm.Picture.ContentType, userEmail);
            await unitOfWork.GetRepository<Image>().AddAsync(image);

            //var imageId = Guid.Parse("3A119C03-576D-4C71-B180-5E67DF74A69E");
            var article = new Article(articleAddVm.Title, articleAddVm.Content, userId, userEmail, articleAddVm.CategoryId, image.Id);
            //var article = new Article
            //{
            //    Title = articleAddVm.Title,
            //    Content = articleAddVm.Content,
            //    CategoryId = articleAddVm.CategoryId,
            //    UserId = userId
            //};

            await unitOfWork.GetRepository<Article>().AddAsync(article);
            await unitOfWork.SaveAsync();
        }

        public async Task<List<ArticleViewModel>> GetAllArticlesWithCategoryNonDeletedAsync()
        {
            var articles = await unitOfWork.GetRepository<Article>().GetAllAsync(x => !x.IsDeleted, x => x.Category);
            var map = mapper.Map<List<ArticleViewModel>>(articles);
            return map;
        }

        public async Task<ArticleViewModel> GetArticleWithCategoryNonDeletedAsync(Guid articleId)
        {
            var article = await unitOfWork.GetRepository<Article>().GetAsync(x => !x.IsDeleted && x.Id == articleId, c => c.Category, i => i.Image, u => u.User);
            var map = mapper.Map<ArticleViewModel>(article);
            return map;
        }

        public async Task<string> UpdateArticleAsync(ArticleUpdateViewModel articleUpdateVm)
        {
            var article = await unitOfWork.GetRepository<Article>().GetAsync(x => !x.IsDeleted && x.Id == articleUpdateVm.Id, c => c.Category, i => i.Image);
            var userEmail = _user.GetLoggedInEmail();


            if (articleUpdateVm.Picture != null)
            {
                imageHelper.Delete(article.Image.FileName);

                var imageUpload = await imageHelper.Upload(articleUpdateVm.Title, articleUpdateVm.Picture, ImageType.Post);
                Image image = new(imageUpload.FullName, articleUpdateVm.Picture.ContentType, userEmail);
                await unitOfWork.GetRepository<Image>().AddAsync(image);

                article.ImageId = image.Id;
                //await unitOfWork.SaveAsync();
            }
            //else
            //    articleUpdateVm.Image = article.Image;

            mapper.Map(articleUpdateVm, article);
            //article.Title = articleUpdateVm.Title;
            //article.Content = articleUpdateVm.Content;
            //article.CategoryId = articleUpdateVm.CategoryId;
            article.ModifiedDate = DateTime.Now;
            article.ModifiedBy = userEmail;

            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();

            return article.Title;
        }

        public async Task<string> SafeDeleteArticleAsync(Guid articleId)
        {
            var article = await unitOfWork.GetRepository<Article>().GetByGuidAsync(articleId);
            var userEmail = _user.GetLoggedInEmail();

            article.IsDeleted = true;
            article.DeleteDate = DateTime.Now;
            article.DeletedBy = userEmail;

            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();

            return article.Title;
        }

        public async Task<List<ArticleViewModel>> GetAllArticlesWithCategoryDeletedAsync()
        {
            var articles = await unitOfWork.GetRepository<Article>().GetAllAsync(x => x.IsDeleted, x => x.Category);
            var map = mapper.Map<List<ArticleViewModel>>(articles);
            return map;
        }

        public async Task<string> UndoDeleteArticleAsync(Guid articleId)
        {
            var article = await unitOfWork.GetRepository<Article>().GetByGuidAsync(articleId);
            var userEmail = _user.GetLoggedInEmail();

            article.IsDeleted = false;
            article.DeleteDate = null;
            article.DeletedBy = null;

            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();

            return article.Title;
        }

        public async Task<ArticleListViewModel> GetAllByPagingAsync(Guid? categoryId, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;

            var articles = categoryId == null
                ? await unitOfWork.GetRepository<Article>().GetAllAsync(x => !x.IsDeleted, c => c.Category, i => i.Image, u => u.User)
                : await unitOfWork.GetRepository<Article>().GetAllAsync(x => x.CategoryId == categoryId && !x.IsDeleted, c => c.Category, i => i.Image, u => u.User);

            var sortedArticles = isAscending
                ? articles.OrderBy(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : articles.OrderByDescending(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new ArticleListViewModel
            {
                Articles = sortedArticles,
                CategoryId = categoryId == null ? null : categoryId.Value,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
                IsAscending = isAscending
            };
        }

        public async Task<ArticleListViewModel> SearchAsync(string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;

            var articles = await unitOfWork.GetRepository<Article>().GetAllAsync(x => !x.IsDeleted
                && (x.Title.Contains(keyword) || x.Content.Contains(keyword) || x.Category.Name.Contains(keyword)),
                c => c.Category, i => i.Image, u => u.User);


            var sortedArticles = isAscending
                ? articles.OrderBy(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : articles.OrderByDescending(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new ArticleListViewModel
            {
                Articles = sortedArticles,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
                IsAscending = isAscending
            };
        }
    }
}
