using CompanyStructureApp.Domain.Core.Interfaces;
using CompanyStructureApp.Domain.Core.Interfaces.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyStructureApp.Domain.Core.Concrete.Visitors
{
    public class EmployeeWithSuperiorVisitor : IEmployeeWithSuperiorVisitor
    {
        public List<IEmployee> EmployeesWithSuperior { get; private set; }

        private string _superiorId { get; set; }

        public EmployeeWithSuperiorVisitor(string superiorId)
        {
            _superiorId = superiorId;
        }
                
        public void VisitEmployeeComposite(EmployeeComposite employeeComposite)
        {
            if (employeeComposite is null)
            {
                throw new ArgumentNullException(nameof(employeeComposite));
            }

            if (employeeComposite.Employee.Id.ToString() == _superiorId)
            {
                EmployeesWithSuperior = employeeComposite
                    .EmployeeComponents.Select(c => c.Employee)
                    .ToList();
            }
        }

        public void VisitEmployeeLeaf(EmployeeLeaf employeeLeaf)
        {
            if (employeeLeaf is null)
            {
                throw new ArgumentNullException(nameof(employeeLeaf));
            }

        }

    }
}
