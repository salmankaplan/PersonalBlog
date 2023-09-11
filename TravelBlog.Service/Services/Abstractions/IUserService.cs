using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBlog.Entity.Entities;
using TravelBlog.Entity.ViewModels.Users;

namespace TravelBlog.Service.Services.Abstractions
{
    public interface IUserService
    {
        Task<List<UserViewModel>> GetAllUsersWithRoleAsync();
        Task<List<AppRole>> GetAllRolesAsync();
        Task<IdentityResult> CreateUserAsync(UserAddViewModel userAddVm);
        Task<IdentityResult> UpdateUserAsync(UserUpdateViewModel userUpdateVm);
        Task<AppUser> GetAppUserByIdAsync(Guid userId);
        Task<string> GetUserRoleAsync(AppUser user);
        Task<(IdentityResult identityResult,string? email)> DeleteUserAsync(Guid userId);
        Task<UserProfileViewModel> GetUserProfileByIdAsync();       
        Task<bool> UserProfileUpdateAsync(UserProfileViewModel userProfileVm);       
    }
}
