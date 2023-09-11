using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelBlog.Data.UnitOfWorks;
using TravelBlog.Entity.Entities;
using TravelBlog.Entity.Enums;
using TravelBlog.Entity.ViewModels.Users;
using TravelBlog.Service.Extensions;
using TravelBlog.Service.Helpers.Images;
using TravelBlog.Service.Services.Abstractions;

namespace TravelBlog.Service.Services.Concretes
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IImageHelper imageHelper;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ClaimsPrincipal _user;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IImageHelper imageHelper, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.imageHelper = imageHelper;
            _user = httpContextAccessor.HttpContext.User;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        public async Task<List<UserViewModel>> GetAllUsersWithRoleAsync()
        {
            var users = await userManager.Users.ToListAsync();
            var map = mapper.Map<List<UserViewModel>>(users);

            foreach (var item in map)
            {
                var findUser = await userManager.FindByIdAsync(item.Id.ToString());
                var role = string.Join("", await userManager.GetRolesAsync(findUser));

                item.Role = role;
            }

            return map;
        }

        public async Task<List<AppRole>> GetAllRolesAsync()
        {
            return await roleManager.Roles.ToListAsync();
        }

        public async Task<IdentityResult> CreateUserAsync(UserAddViewModel userAddVm)
        {
            var map = mapper.Map<AppUser>(userAddVm);
            map.UserName = userAddVm.Email;

            var result = await userManager.CreateAsync(map, string.IsNullOrEmpty(userAddVm.Password) ? "" : userAddVm.Password);
            if (result.Succeeded)
            {
                var findRole = await roleManager.FindByIdAsync(userAddVm.RoleId.ToString());
                await userManager.AddToRoleAsync(map, findRole.ToString());
                return result;
            }
            else
                return result;
        }

        public async Task<IdentityResult> UpdateUserAsync(UserUpdateViewModel userUpdateVm)
        {
            var user = await GetAppUserByIdAsync(userUpdateVm.Id);
            var userRole = await GetUserRoleAsync(user);

            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await userManager.RemoveFromRoleAsync(user, userRole);
                var findRole = await roleManager.FindByIdAsync(userUpdateVm.RoleId.ToString());
                await userManager.AddToRoleAsync(user, findRole.Name);
                return result;
            }
            else
                return result;
        }

        public async Task<AppUser> GetAppUserByIdAsync(Guid userId)
        {
            return await userManager.FindByIdAsync(userId.ToString());
        }

        public async Task<string> GetUserRoleAsync(AppUser user)
        {
            return string.Join("", await userManager.GetRolesAsync(user));
        }

        public async Task<(IdentityResult identityResult, string? email)> DeleteUserAsync(Guid userId)
        {
            var user = await GetAppUserByIdAsync(userId);
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
                return (result, user.Email);
            else
                return (result, null);
        }

        public async Task<UserProfileViewModel> GetUserProfileByIdAsync()
        {
            var userId = _user.GetLoggedInUserId();
            var getUserwithImage = await unitOfWork.GetRepository<AppUser>().GetAsync(x => x.Id == userId, x => x.Image);
            var map = mapper.Map<UserProfileViewModel>(getUserwithImage);
            map.Image.FileName = getUserwithImage.Image.FileName;

            return map;
        }

        public async Task<bool> UserProfileUpdateAsync(UserProfileViewModel userProfileVm)
        {
            var userId = _user.GetLoggedInUserId();
            var user = await GetAppUserByIdAsync(userId);

            var isVerified = await userManager.CheckPasswordAsync(user, userProfileVm.CurrentPassword);
            if (isVerified && userProfileVm.NewPassword != null)
            {
                var result = await userManager.ChangePasswordAsync(user, userProfileVm.CurrentPassword, userProfileVm.NewPassword);
                if (result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user);
                    await signInManager.SignOutAsync();
                    await signInManager.PasswordSignInAsync(user, userProfileVm.NewPassword, true, false);

                    mapper.Map(userProfileVm, user);

                    if (userProfileVm.Picture != null)
                        user.ImageId = await UploadImageForUserAsync(userProfileVm);

                    await userManager.UpdateAsync(user);
                    await unitOfWork.SaveAsync();

                    return true;
                }
                else
                    return false;
            }
            else if (isVerified)
            {
                await userManager.UpdateSecurityStampAsync(user);
                mapper.Map(userProfileVm, user);

                if (userProfileVm.Picture != null)
                    user.ImageId = await UploadImageForUserAsync(userProfileVm);

                await userManager.UpdateAsync(user);
                await unitOfWork.SaveAsync();

                return true;
            }
            else
                return false;

        }

        private async Task<Guid> UploadImageForUserAsync(UserProfileViewModel userProfileVm)
        {
            var userEmail = _user.GetLoggedInEmail();

            var imageUpload = await imageHelper.Upload($"{userProfileVm.FirstName}{userProfileVm.LastName}", userProfileVm.Picture, ImageType.User);
            Image image = new(imageUpload.FullName, userProfileVm.Picture.ContentType, userEmail);
            await unitOfWork.GetRepository<Image>().AddAsync(image);

            return image.Id;
        }
    }
}
