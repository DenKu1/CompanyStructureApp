using CompanyStructureApp.Domain.Core.Abstract;
using CompanyStructureApp.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace CompanyStructureApp.Domain.Interfaces.Repositories
{
    public interface ICompanyStructureRepository
    {
        //видалений непотрібний паблік
        string AddEmployee(IEmployee employee, string superiorEmployeeId);
        //видалений непотрібний паблік
        List<IEmployee> GetEmployees(Func<EmployeeComponent, bool> function);
        //видалений непотрібний паблік
        IEmployee GetEmployeeById(string id);
        List<IEmployee> FindEmployeesWithSuperior(string superiorId);
        List<IEmployee> FindEmployeesWithBiggestSalary();
        List<IEmployee> GetAllEmployees();
    }
}

