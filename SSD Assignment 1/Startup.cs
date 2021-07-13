﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SSD_Assignment_1.Data;
using SSD_Assignment_1.Models;
using Stripe;

namespace SSD_Assignment_1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SSD_Assignment_1Context>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                //.AddEntityFrameworkStores<SSD_Assignment_1Context>();
            services.AddRazorPages();

            services.AddDbContext<SSD_Assignment_1Context>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("SSD_Assignment_1Context")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultUI()
                .AddEntityFrameworkStores<SSD_Assignment_1Context>()
                .AddDefaultTokenProviders();

            services.AddNotyf(config => 
                               {config.DurationInSeconds = 10; 
                                config.IsDismissable = true; 
                                config.Position = NotyfPosition.TopRight; });


            services.AddMvc()
            .AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizeFolder("/Admin");
                //options.Conventions.AuthorizeFolder("/Admin/Products");
                //options.Conventions.AuthorizeFolder("/Admin/Roles");
                //options.Conventions.AuthorizeFolder("/Admin/Audit");
      
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // options.Cookie.Name = "YourCookieName";
                //  options.Cookie.Domain=
                // options.LoginPath = "/Account/Login";
                // options.LogoutPath = "/Account/Logout";
                // options.AccessDeniedPath = "/Account/AccessDenied";

                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromSeconds(500);
                options.SlidingExpiration = true;
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
               app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseStatusCodePages("text/html", "<h1>Oops,seems like page cannot be found!</h1><h1>Status code page</h1> <h2>Status Code: {0}</h2>");
                app.UseExceptionHandler("/Error");
            }
            else
            {
                //app.UseStatusCodePages("text/html", "<h1>Status code page</h1> <h2>Status Code: {0}</h2>");
                //app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            StripeConfiguration.ApiKey = "sk_test_51JBK5eHn9TQEqDEm3YLtX4izFQsOJrEpj1aVxtibQ9dWLiYyIx6DVkq5fMTNkIZclnQ7tfG2NyYbnvbCsrDC6HAZ00RsSK48uQ";

            app.UseNotyf();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
