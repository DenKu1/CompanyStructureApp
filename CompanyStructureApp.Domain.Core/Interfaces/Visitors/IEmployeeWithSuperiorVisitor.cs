using System.Collections.Generic;

namespace CompanyStructureApp.Domain.Core.Interfaces.Visitors
{
    public interface IEmployeeWithSuperiorVisitor : IEmployeeVisitor
    {   
        List<IEmployee> EmployeesWithSuperior { get; }
    }
}