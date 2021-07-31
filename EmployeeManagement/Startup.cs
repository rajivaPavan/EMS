using System;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region Adding DbContext
            //var serverVersion = new MySqlServerVersion(new Version(8, 0, 25));
            //services.AddDbContextPool<AppDbContext>(
            //    options => options.UseMySql(_config.GetConnectionString("EmployeeDBConnection"), serverVersion));

            services.AddDbContext<AppDbContext>(); //
            #endregion

            services.AddIdentity<IdentityUser, IdentityRole>(options => {
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 3;
            }).AddEntityFrameworkStores<AppDbContext>();

            
            services.AddMvc(options => options.EnableEndpointRouting = false).AddXmlSerializerFormatters();

            #region Adding Repositories
            //services.AddScoped<IEmployeeRepository, MySqlEmployeeRepository>();
            services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>();
            #endregion

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
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseExceptionHandler("/Error");

            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
