using CompanyStructureApp.Domain.Core.Abstract;
using CompanyStructureApp.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace CompanyStructureApp.Domain.Interfaces.Repositories
{
    public interface ICompanyStructureRepository
    {
        public string AddEmployee(IEmployee employee, string superiorEmployeeId);
        public List<IEmployee> GetEmployees(Func<EmployeeComponent, bool> function);
        public IEmployee GetEmployeeById(string id);
        List<IEmployee> FindEmployeesWithSuperior(string superiorId);
        List<IEmployee> FindEmployeesWithBiggestSalary();
        List<IEmployee> GetAllEmployees();
    }
}

