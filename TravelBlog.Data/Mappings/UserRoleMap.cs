using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBlog.Entity.Entities;

namespace TravelBlog.Data.Mappings
{
    public class UserRoleMap : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder.HasKey(r => new { r.UserId, r.RoleId });

            // Maps to the AspNetUserRoles table
            builder.ToTable("AppUserRoles");

            builder.HasData(new AppUserRole
            {
                UserId = Guid.Parse("391D592B-EED1-4E72-94AB-10B341D0739C"),
                RoleId = Guid.Parse("EA808ACE-2727-4AEF-B764-173B5A414630")
            },
            new AppUserRole
            {
                UserId= Guid.Parse("6A7E0458-79D2-4E4D-A34C-609CE6CA1C00"),
                RoleId= Guid.Parse("55D0C696-1C10-4324-A319-D4175F937AA8")
            }
            );
        }
    }
}
