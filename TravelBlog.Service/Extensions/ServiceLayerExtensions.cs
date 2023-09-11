using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;
using TravelBlog.Service.FluentValidations;
using TravelBlog.Service.Helpers.Images;
using TravelBlog.Service.Services.Abstractions;
using TravelBlog.Service.Services.Concretes;

namespace TravelBlog.Service.Extensions
{
    public static class ServiceLayerExtensions
    {
        public static IServiceCollection LoadServiceLayerExtension(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddScoped<IDashboardService, DashboardService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAutoMapper(assembly);

            services.AddControllersWithViews().AddFluentValidation(option =>
            {
                option.RegisterValidatorsFromAssemblyContaining<ArticleValidator>();
                option.DisableDataAnnotationsValidation = true;
                option.ValidatorOptions.LanguageManager.Culture = new CultureInfo("tr");
            });

            return services;
        }
    }
}
