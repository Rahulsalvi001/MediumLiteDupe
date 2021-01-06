using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogicLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Utility;
using DataAccessLayer.Data;
using EntityModels;
using MediumLiteDupe.Helper;
using DataAccessLayer.Repository.IRepository;
using DataAccessLayer.Repository;

namespace MediumLiteDupe
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
            services.AddControllersWithViews();
            services.Configure<EmailSenderOptions>(Configuration.GetSection("SendGrid"));
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddDefaultTokenProviders()
            //.AddEntityFrameworkStores<AppDBContext>().AddUserManager<UserManager<IdentityUser>>();
            services.AddIdentity<AppUser, IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<AppDBContext>();
            services.AddSingleton<IEmailSender, EmailSender>();
            // Added Custom user principle to get access of extended props from AppUser in identity
            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, MyUserClaimsPrincipalFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddRazorPages();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            // Make sure you call this before calling app.UseMvc()
            app.UseCors(
                options => options.WithOrigins("http://localhost:8008/uploadFile", "http://localhost:8008/fetchUrl").AllowAnyMethod()
            );
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Dashoard}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
