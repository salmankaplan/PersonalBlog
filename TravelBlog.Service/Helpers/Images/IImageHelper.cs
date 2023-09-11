using Microsoft.AspNetCore.Http;
using TravelBlog.Entity.Enums;
using TravelBlog.Entity.ViewModels.Images;

namespace TravelBlog.Service.Helpers.Images
{
    public interface IImageHelper
    {
        Task<ImageUploadViewModel> Upload(string name, IFormFile imageFile,ImageType imageType,string folderName = null);
        void Delete(string imageName);
    }
}
