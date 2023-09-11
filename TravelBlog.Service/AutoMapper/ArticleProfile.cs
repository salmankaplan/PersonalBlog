using AutoMapper;
using TravelBlog.Entity.ViewModels.Articles;
using TravelBlog.Entity.Entities;

namespace TravelBlog.Service.AutoMapper
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<ArticleViewModel, Article>().ReverseMap();
            CreateMap<ArticleUpdateViewModel, Article>().ReverseMap();
            CreateMap<ArticleUpdateViewModel, ArticleViewModel>().ReverseMap();
            CreateMap<ArticleAddViewModel, Article>().ReverseMap();
        }
    }
}
