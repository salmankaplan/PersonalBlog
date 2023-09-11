using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBlog.Entity.Entities;

namespace TravelBlog.Data.Mappings
{
    public class ArticleVisitorMap : IEntityTypeConfiguration<ArticleVisitor>
    {
        public void Configure(EntityTypeBuilder<ArticleVisitor> builder)
        {
            builder.HasKey(x => new { x.ArticleId, x.VisitorId });
        }
    }
}
