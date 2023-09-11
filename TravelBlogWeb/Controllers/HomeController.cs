using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TravelBlog.Data.UnitOfWorks;
using TravelBlog.Entity.Entities;
using TravelBlog.Service.Services.Abstractions;
using TravelBlog.Web.Models;

namespace TravelBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService articleService;
        private readonly IHttpContextAccessor httpContext;
        private readonly IUnitOfWork unitOfWork;

        public HomeController(ILogger<HomeController> logger, IArticleService articleService, IHttpContextAccessor httpContext, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.articleService = articleService;
            this.httpContext = httpContext;
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> Index(Guid? categoryId, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            var articles = await articleService.GetAllByPagingAsync(categoryId, currentPage, pageSize, isAscending);
            return View(articles);
        }
        [HttpGet]
        public async Task<IActionResult> Search(string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            var articles = await articleService.SearchAsync(keyword, currentPage, pageSize, isAscending);
            return View(articles);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var ipAdress = httpContext.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var articleVisitors = await unitOfWork.GetRepository<ArticleVisitor>().GetAllAsync(null, x => x.Visitor, y => y.Article);
            var article = await unitOfWork.GetRepository<Article>().GetAsync(x => x.Id == id);

            var result = await articleService.GetArticleWithCategoryNonDeletedAsync(id);

            var visitor = await unitOfWork.GetRepository<Visitor>().GetAsync(x => x.IpAdress == ipAdress);
            var addArticleVistor = new ArticleVisitor(article.Id, visitor.Id);
            if (articleVisitors.Any(x => x.VisitorId == addArticleVistor.VisitorId && x.ArticleId == addArticleVistor.ArticleId))
                return View(result);
            else
            {
                await unitOfWork.GetRepository<ArticleVisitor>().AddAsync(addArticleVistor);
                article.ViewCount += 1;
                await unitOfWork.GetRepository<Article>().UpdateAsync(article);
                await unitOfWork.SaveAsync();
            }

            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}