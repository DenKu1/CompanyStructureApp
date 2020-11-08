using AutoMapper;
using CompanyStructureApp.Domain.Core.Concrete;
using CompanyStructureApp.Domain.Core.Interfaces;
using CompanyStructureApp.Domain.Interfaces.Repositories;
using CompanyStructureApp.Domain.Interfaces.Services;
using CompanyStructureApp.Infrastructure.DataAccess;
using CompanyStructureApp.Infrastructure.Services;
using CompanyStructureApp.WEB.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CompanyStructureApp.WEB
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

            services.AddAutoMapper(typeof(MapperProfile));

            services.AddSingleton<ICompanyStructureService, CompanyStructureService>();
            services.AddSingleton<IEmployeeService, EmployeeService>();

            services.AddSingleton<ICompanyStructureRepository, CompanyStructureRepository>();
            services.AddSingleton<ICompanyStructureContainer, CompanyStructureContainer>();

            services.AddSingleton<IEmployeeComponentFactory, EmployeeComponentFactory>();
            services.AddSingleton<IEmployee, Employee>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
