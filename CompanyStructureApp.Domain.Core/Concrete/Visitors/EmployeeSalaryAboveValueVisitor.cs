using CompanyStructureApp.Domain.Core.Abstract;
using CompanyStructureApp.Domain.Core.Interfaces;
using CompanyStructureApp.Domain.Core.Interfaces.Visitors;
using System.Collections.Generic;

namespace CompanyStructureApp.Domain.Core.Concrete.Visitors
{
    class EmployeeSalaryAboveValueVisitor : IEmployeeSalaryAboveValueVisitor
    {
        public List<IEmployee> EmployeesWithSalaryAboveValue { get; private set; }

        private int _salary;

        public EmployeeSalaryAboveValueVisitor(int salary)
        {
            EmployeesWithSalaryAboveValue = new List<IEmployee>();

            _salary = salary;
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
            if (employeeComponent == null)
            {
                throw null;
            }

            if (employeeComponent.Employee.Salary > _salary)
            {
                EmployeesWithSalaryAboveValue.Add(employeeComponent.Employee);
            }
        }
    }
}
