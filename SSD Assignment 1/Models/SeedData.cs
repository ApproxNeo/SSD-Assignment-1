using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SSD_Assignment_1.Data;

namespace SSD_Assignment_1.Models
{
    public class SeedData
    {
        private readonly IServiceProvider _serviceProvider;

        public SeedData(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async void Seeding(IServiceProvider serviceProvider)
        {

            var context = new SSD_Assignment_1Context(serviceProvider.GetRequiredService<DbContextOptions<SSD_Assignment_1Context>>());
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            var user = new ApplicationUser { UserName = "Admin@gmail.com", Email = "Admin@gmail.com", BirthDate = DateTime.Now }; 
            var result = await userManager.CreateAsync(user, "Admin123");
        }
    }
}
