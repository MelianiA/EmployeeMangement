using EmployeeMangement.Models;
using EmployeeMangement.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EmployeeMangement
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddDirectoryBrowser();
            services.AddMvc(
                options =>
                        {
                            options.EnableEndpointRouting = false;
                            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                                                         .Build();
                            options.Filters.Add(new AuthorizeFilter(policy));
                        }).AddXmlSerializerFormatters();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddDirectoryBrowser();

            services.AddScoped<ICompanyRepository<Employee>, SQLEmployeeRepository>();
            services.ConfigureApplicationCookie(op => op.LoginPath = "/Account/Login");

            services.AddDbContext<AppDbContext>(
                optionsAction => optionsAction.UseSqlServer(
                    _configuration.GetConnectionString("EmployeeDbConnection")));

            services.AddIdentity<AppUser, IdentityRole>(options =>
           {
               options.Password.RequireUppercase = false;
               options.Password.RequireLowercase = false;
           }).AddEntityFrameworkStores<AppDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();

            //app.UseFileServer(new FileServerOptions
            //{
            //    FileProvider = new PhysicalFileProvider("/"),
            //    RequestPath = new PathString("/ab/"),
            //    EnableDirectoryBrowsing = false
            //});

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                            Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "images")),
                RequestPath = new PathString("/MyImages")
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images")),
                RequestPath = new PathString("/MyImages")
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default",
                    "{controller=Employee}/{action=Index}/{id?}");
            });
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("hello world");
                });

            });
        }
    }
}
