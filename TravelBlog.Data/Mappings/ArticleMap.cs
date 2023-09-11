using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBlog.Entity.Entities;

namespace TravelBlog.Data.Mappings
{
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasData(new Article
            {
                Id = Guid.NewGuid(),
                Title = "Aspnet Core Deneme Makalesi 1",
                Content = "Aspnet Core fames convallis velit vehicula neque commodo at ridiculus ultrices augue maecenas egestas eu a nulla phasellus viverra eleifend",
                ViewCount = 14,
                CategoryId = Guid.Parse("D200BB97-7952-4C15-BB5A-00463E3E1F0C"),
                ImageId = Guid.Parse("3A119C03-576D-4C71-B180-5E67DF74A69E"),
                CreatedBy = "Admin Test",
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                UserId = Guid.Parse("391D592B-EED1-4E72-94AB-10B341D0739C")
            },
            new Article
            {
                Id = Guid.NewGuid(),
                Title = "Visual Studio Deneme Makalesi 1",
                Content = "Visual Studio fames convallis velit vehicula neque commodo at ridiculus ultrices augue maecenas egestas eu a nulla phasellus viverra eleifend",
                ViewCount = 14,
                CategoryId = Guid.Parse("04AACA8B-5522-462F-AB6E-471717B04052"),
                ImageId = Guid.Parse("0C8F4329-3779-4848-959D-21A44658B6EE"),
                CreatedBy = "Admin Test",
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                UserId = Guid.Parse("6A7E0458-79D2-4E4D-A34C-609CE6CA1C00")
            }
            );
        }
    }
}
