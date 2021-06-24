using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using SSD_Assignment_1.Models;

namespace SSD_Assignment_1.Data
{

    public class SSD_Assignment_1Context : IdentityDbContext<ApplicationUser>
    {
        public SSD_Assignment_1Context (DbContextOptions<SSD_Assignment_1Context> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }


        public DbSet<SSD_Assignment_1.Models.Product> Product { get; set; }
    }
}
