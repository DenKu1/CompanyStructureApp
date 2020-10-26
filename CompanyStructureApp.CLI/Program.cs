using AutoMapper;
using CompanyStructureApp.CLI.ConsoleApp;
using CompanyStructureApp.CLI.Mapper;
using CompanyStructureApp.Domain.Core.Concrete;
using CompanyStructureApp.Domain.Core.Concrete.Visitors;
using CompanyStructureApp.Domain.Core.Interfaces;
using CompanyStructureApp.Domain.Core.Interfaces.Visitors;
using CompanyStructureApp.Domain.Interfaces.Repositories;
using CompanyStructureApp.Domain.Interfaces.Services;
using CompanyStructureApp.Infrastructure.DataAccess;
using CompanyStructureApp.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CompanyStructureApp.CLI
{
    class Program
    {
        private static IServiceProvider _serviceProvider;

        static void Main()
        {
            RegisterServices();
            IServiceScope scope = _serviceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<ConsoleApplication>().Run();
            DisposeServices();
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();

            var mapperConfig = new MapperConfiguration(c => c.AddProfile(new MapperProfile()));

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSingleton<ConsoleApplication>();

            services.AddSingleton<ICompanyStructureService, CompanyStructureService>();
            services.AddSingleton<IEmployeeService, EmployeeService>();

            services.AddSingleton<ICompanyStructureRepository, CompanyStructureRepository>();
            services.AddSingleton<ICompanyStructureContainer, CompanyStructureContainer>();

            services.AddSingleton<IEmployeeComponentFactory, EmployeeComponentFactory>();
            services.AddSingleton<IEmployee, Employee>();

            //services.AddTransient<IEmployeeWithSuperiorVisitor, EmployeeWithSuperiorVisitor>();
            //services.AddTransient<IEmployeeMaxSalaryVisitor, EmployeeMaxSalaryVisitor>();

            _serviceProvider = services.BuildServiceProvider(true);
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

    }
}
