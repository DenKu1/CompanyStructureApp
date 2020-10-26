using System.Collections.Generic;

namespace CompanyStructureApp.Domain.Core.Interfaces.Visitors
{
    interface IEmployeeSalaryAboveValueVisitor : IEmployeeVisitor
    {
        List<IEmployee> EmployeesWithSalaryAboveValue { get; }
    }
}