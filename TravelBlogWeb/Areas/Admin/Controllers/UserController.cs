using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Data;
using TravelBlog.Data.UnitOfWorks;
using TravelBlog.Entity.Entities;
using TravelBlog.Entity.Enums;
using TravelBlog.Entity.ViewModels.Users;
using TravelBlog.Service.Extensions;
using TravelBlog.Service.Helpers.Images;
using TravelBlog.Service.Services.Abstractions;
using TravelBlog.Web.Consts;
using TravelBlog.Web.ResultMessages;
using static TravelBlog.Web.ResultMessages.Messages;

namespace TravelBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IValidator<AppUser> validator;
        private readonly IToastNotification toast;
        private readonly IMapper mapper;

        public UserController(IUserService userService, IValidator<AppUser> validator, IToastNotification toast, IMapper mapper)
        {
            this.userService = userService;
            this.validator = validator;
            this.toast = toast;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var result = await userService.GetAllUsersWithRoleAsync();

            return View(result);
        }
        [HttpGet]
        //[Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Add()
        {
            var roles = await userService.GetAllRolesAsync();
            return View(new UserAddViewModel { Roles = roles });
        }
        [HttpPost]
        //[Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Add(UserAddViewModel userAddVm)
        {
            var map = mapper.Map<AppUser>(userAddVm);
            var valiadation = await validator.ValidateAsync(map);
            var roles = await userService.GetAllRolesAsync();

            if (ModelState.IsValid)
            {
                var result = await userService.CreateUserAsync(userAddVm);
                if (result.Succeeded)
                {

                    toast.AddSuccessToastMessage(Messages.User.Add(userAddVm.Email), new ToastrOptions { Title = "Başarılı" });
                    return RedirectToAction("Index", "User", new { Area = "Admin" });
                }
                else
                {
                    result.AddToIdentityModelState(this.ModelState);
                    valiadation.AddToModelState(this.ModelState);
                    return View(new UserAddViewModel { Roles = roles });
                }
            }
            return View(new UserAddViewModel { Roles = roles });
        }
        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Update(Guid userId)
        {
            var user = await userService.GetAppUserByIdAsync(userId);
            var roles = await userService.GetAllRolesAsync();

            var map = mapper.Map<UserUpdateViewModel>(user);
            map.Roles = roles;

            return View(map);
        }
        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Update(UserUpdateViewModel userUpdateVm)
        {
            var user = await userService.GetAppUserByIdAsync(userUpdateVm.Id);

            if (user != null)
            {
                var roles = await userService.GetAllRolesAsync();
                if (ModelState.IsValid)
                {
                    var map = mapper.Map(userUpdateVm, user);
                    var valiadation = await validator.ValidateAsync(map);

                    if (valiadation.IsValid)
                    {
                        user.UserName = userUpdateVm.Email;
                        user.SecurityStamp = Guid.NewGuid().ToString();
                        var result = await userService.UpdateUserAsync(userUpdateVm);

                        if (result.Succeeded)
                        {
                            toast.AddSuccessToastMessage(Messages.User.Update(userUpdateVm.Email), new ToastrOptions { Title = "İşlem Başarılı" });
                            return RedirectToAction("Index", "User", new { Area = "Admin" });
                        }
                        else
                        {
                            result.AddToIdentityModelState(this.ModelState);
                            return View(new UserUpdateViewModel { Roles = roles });
                        }
                    }
                    else
                    {
                        valiadation.AddToModelState(this.ModelState);
                        return View(new UserUpdateViewModel { Roles = roles });
                    }
                }
            }
            return NotFound();
        }
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var result = await userService.DeleteUserAsync(userId);
            if (result.identityResult.Succeeded)
            {
                toast.AddSuccessToastMessage(Messages.User.Delete(result.email), new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }
            else
            {
                result.identityResult.AddToIdentityModelState(this.ModelState);
            }
            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var profile = await userService.GetUserProfileByIdAsync();

            return View(profile);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileViewModel userProfileVm)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.UserProfileUpdateAsync(userProfileVm);
                if (result)
                {
                    toast.AddSuccessToastMessage("Profil güncelleme işlemi tamamlandı", new ToastrOptions { Title = "İşlem Başarılı!" });
                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                }
                else
                {
                    var profile = await userService.GetUserProfileByIdAsync();
                    toast.AddErrorToastMessage("Profil güncelleme işlemi tamamlanamadı", new ToastrOptions { Title = "İşlem Başarısız!" });
                    return View(profile);
                }
            }
            else
                return NotFound();
        }
    }
}
