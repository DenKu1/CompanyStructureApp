using System.Collections.Generic;

namespace CompanyStructureApp.Domain.Core.Interfaces.Visitors
{
    public interface IEmployeeMaxSalaryVisitor : IEmployeeVisitor
    {
        List<IEmployee> EmployeesWithMaxSalary { get; }
    }
}