using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBlog.Entity.BaseEntity;

namespace TravelBlog.Entity.Entities
{
    public class AppUser : IdentityUser<Guid>, IEntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid ImageId { get; set; } = Guid.Parse("01750510-3985-4409-a3ad-df34520f7408");
        public Image Image { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
