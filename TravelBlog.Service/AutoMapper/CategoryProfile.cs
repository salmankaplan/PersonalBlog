using AutoMapper;
using TravelBlog.Entity.Entities;
using TravelBlog.Entity.ViewModels.Categories;

namespace TravelBlog.Service.AutoMapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryViewModel, Category>().ReverseMap();
            CreateMap<CategoryAddViewModel, Category>().ReverseMap();
            CreateMap<CategoryUpdateViewModel, Category>().ReverseMap();
        }
    }
}
