using CompanyStructureApp.Domain.Core.Abstract;
using CompanyStructureApp.Domain.Core.Interfaces;
using System.Collections.Generic;
using System;
using CompanyStructureApp.Domain.Core.Interfaces.Visitors;

namespace CompanyStructureApp.Domain.Core.Concrete.Visitors
{
    public class EmployeeMaxSalaryVisitor : IEmployeeMaxSalaryVisitor
    {
        private int maxSalary;

        public List<IEmployee> EmployeesWithMaxSalary { get; private set; }

        public EmployeeMaxSalaryVisitor()
        {
            EmployeesWithMaxSalary = new List<IEmployee>();
        }        

        public void VisitEmployeeComposite(EmployeeComposite employeeComposite)
        {
            VisitEmployeeComponent(employeeComposite);
        }

        public void VisitEmployeeLeaf(EmployeeLeaf employeeLeaf)
        {
            VisitEmployeeComponent(employeeLeaf);
        }

        private void VisitEmployeeComponent(EmployeeComponent employeeComponent)
        {
            if (employeeComponent is null)
            {
                throw new ArgumentNullException(nameof(employeeComponent));
            }

            if (EmployeesWithMaxSalary.Count == 0)
            {
                maxSalary = employeeComponent.Employee.Salary;
                EmployeesWithMaxSalary.Add(employeeComponent.Employee);

                return;
            }

            if (maxSalary == employeeComponent.Employee.Salary)
            {
                EmployeesWithMaxSalary.Add(employeeComponent.Employee);
            }
            else if (maxSalary < employeeComponent.Employee.Salary)
            {
                maxSalary = employeeComponent.Employee.Salary;

                EmployeesWithMaxSalary.Clear();
                EmployeesWithMaxSalary.Add(employeeComponent.Employee);
            }
        }
    }
}
