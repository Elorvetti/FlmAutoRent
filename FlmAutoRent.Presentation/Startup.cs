using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FlmAutoRent.Data;
using FlmAutoRent.Services;

namespace FlmAutoRent.Presentation
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
            
            services.AddDbContext<FlmAutoRentContext>( o => {
                o.UseSqlServer(Configuration.GetConnectionString("FlmAutoRentDatabase"));
            });

            //Custom services
            services.AddScoped<FlmAutoRentContext>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IProfilingGroupServices, ProfilingGroupServices>();
            services.AddScoped<IEmailServices, EmailServices>();
            services.AddScoped<IOperatorServices, OperatorServices>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IImageService, ImageServices>();
            services.AddScoped<IVideoService, VideoServices>();
            services.AddScoped<IAttachmentService, AttachmentServices>();
            services.AddScoped<INewsServices, NewsServices>();
            services.AddScoped<IVehiclesBrandsServices, VehiclesBrandsServices>();
            services.AddScoped<ICarServices, CarServices>();
            services.AddScoped<IHomePageService, HomePageService>();
            services.AddScoped<IRewriteService, RewriteService>();
            services.AddScoped<IPeopleService, PeopleService>();
            services.AddScoped<ILogServices, LogServices>();
            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
                options.LoginPath = "/Admin/Account/Login";
                options.AccessDeniedPath = "/Admin/Account/Login";
            });
            services.AddDataProtection();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Areas/Admin/wwwroot")),
                RequestPath = new PathString("/adminroot")
            });

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Admin",
                    pattern: "{area:exists}/{controller=Account}/{action=Login}/{id?}");
                
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            
        }
    }
}
