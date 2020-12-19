using CompanyStructureApp.Domain.Core.Abstract;
using CompanyStructureApp.Domain.Core.Concrete;
using CompanyStructureApp.Domain.Core.Concrete.Visitors;
using CompanyStructureApp.Domain.Core.Exceptions;
using CompanyStructureApp.Domain.Core.Interfaces;
using CompanyStructureApp.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyStructureApp.Infrastructure.DataAccess
{
    public class CompanyStructureRepository : ICompanyStructureRepository
    {
        private readonly ICompanyStructureContainer _container;
        private readonly IEmployeeComponentFactory _factory;

        public CompanyStructureRepository(ICompanyStructureContainer companyStructureContainer, IEmployeeComponentFactory employeeComponentFactory)
        {
            _container = companyStructureContainer;
            _factory = employeeComponentFactory;
        }

        public string AddEmployee(IEmployee employee, string superiorEmployeeId)
        {
            if (superiorEmployeeId is null)
            {
                // If superiorEmployeeId is empty means that we have to change root element to the new one
                _container.RootElement = _factory.CreateEmployeeComponent(employee);
                return employee.Id.ToString();
            }

            EmployeeComponent superiorEmployee = GetEmployeeComponentById(superiorEmployeeId);

            if (superiorEmployee is null)
            { 
                throw new EmployeeException($"Superior employee with id {superiorEmployeeId} is not found");
            }

            if (!(superiorEmployee is EmployeeComposite superiorComposite))
            {
                //TODO: Create additional exception for tree
                throw new EmployeeException($"Superior employee component {superiorEmployee.DisplayInfo()} must be composite");
            }

            EmployeeComponent employeeComponent = _factory.CreateEmployeeComponent(employee);

            superiorComposite.Add(employeeComponent);

            return employee.Id.ToString();
        }

        public IEmployee GetEmployeeById(string id)
        {
            if (_container.RootElement is null)
            {
                return null;
            }

            return GetEmployeeComponentById(id).Employee;
        }

        public List<IEmployee> GetEmployees(Func<EmployeeComponent, bool> function)
        {
            if (_container.RootElement == null)
            {
                return null;
            }

            return _container.Where(function).Select(component => component.Employee).ToList();            
        }

        public List<IEmployee> GetAllEmployees()
        {    
            if (_container.RootElement == null)
            {
                return null;
            }

            return _container.Select(component => component.Employee).ToList();
        }


        public List<IEmployee> FindEmployeesWithBiggestSalary()
        {
            if (_container.RootElement is null)
            {
                return null;
            }

            var visitor = new EmployeeMaxSalaryVisitor();

            _container.RootElement.Accept(visitor);

            return visitor.EmployeesWithMaxSalary;        
        }

        public List<IEmployee> FindEmployeesWithSuperior(string superiorId)
        {
            if (_container.RootElement is null)
            {
                return null;
            }

            var visitor = new EmployeeWithSuperiorVisitor(superiorId);

            _container.RootElement.Accept(visitor);

            return visitor.EmployeesWithSuperior;
        }

        private EmployeeComponent GetEmployeeComponentById(string id)
        {

            if (_container.RootElement is null)
            {
                return null;
            }

            return _container.Where(e => e.Employee.Id.ToString() == id).FirstOrDefault();
        }

        
    }
}
