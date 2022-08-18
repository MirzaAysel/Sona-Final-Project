using Business.Implementations;
using Business.Services;
using DAL.Abstracts;
using DAL.Data;
using DAL.Implementations;
using DAL.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SONA
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            
            // Rooms
            services.AddScoped<IRoomService, RoomRepository>();
            services.AddScoped<IRoomDal, EFRoomRepository>();

            // Blogs
            services.AddScoped<IBlogService, BlogRepository>();
            services.AddScoped<IBlogDal, EfBlogRepository>();

            // Comments
            services.AddScoped<ICommentService, CommentRepository>();
            services.AddScoped<ICommentDal, EfCommentRepository>();
            
            // Settings
            services.AddScoped<ISettingService, SettingRepository>();
            services.AddScoped<ISettingDal, EFSettingRepository>();

            services.AddDbContext<AppDbContext>(op =>
            {
                op.UseSqlServer(_configuration.GetConnectionString("Default"),
                    b => { b.MigrationsAssembly("DAL"); });
            });
            services.AddIdentity<AppUser, IdentityRole>(op =>
            {
                op.Password.RequiredLength = 8;
                op.Lockout.MaxFailedAccessAttempts = 3;
                op.User.RequireUniqueEmail = true;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthentication();
            services.AddAuthorization();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("admin", "{area:exists}/{controller=dashboard}/{action=index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=home}/{action=index}/{id?}");
            });
        }
    }
}