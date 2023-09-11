using AutoMapper;
using TravelBlog.Entity.Entities;
using TravelBlog.Entity.ViewModels.Users;

namespace TravelBlog.Service.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, UserViewModel>().ReverseMap();
            CreateMap<AppUser, UserAddViewModel>().ReverseMap();
            CreateMap<AppUser, UserUpdateViewModel>().ReverseMap();
            CreateMap<AppUser, UserProfileViewModel>().ReverseMap();
        }
    }
}
