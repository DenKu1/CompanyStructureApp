using CompanyStructureApp.Domain.Core.Abstract;
using CompanyStructureApp.Domain.Core.Interfaces;
using CompanyStructureApp.Domain.Core.Interfaces.Visitors;
using System.Collections.Generic;
using System;

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
            //винесена валидация в метод
            ifNullThenThrowArgumentNullException(employeeComponent);

            if (EmployeesWithMaxSalary.Count == 0)
            {
                choseFirstUserWhenNoUserInList(employeeComponent);
                return;
            }

            if (maxSalary == employeeComponent.Employee.Salary)
            {
                EmployeesWithMaxSalary.Add(employeeComponent.Employee);
            }
            else if (maxSalary < employeeComponent.Employee.Salary)
            {
                ClearListWhenFindUserWithBiggerSalaryThenCurrent(employeeComponent);
            }
        }

        private void ClearListWhenFindUserWithBiggerSalaryThenCurrent(EmployeeComponent employeeComponent)
        {
            maxSalary = employeeComponent.Employee.Salary;

            EmployeesWithMaxSalary.Clear();
            EmployeesWithMaxSalary.Add(employeeComponent.Employee);
        }

        private void choseFirstUserWhenNoUserInList(EmployeeComponent employeeComponent)
        {
            maxSalary = employeeComponent.Employee.Salary;
            EmployeesWithMaxSalary.Add(employeeComponent.Employee);
        }

        private static void ifNullThenThrowArgumentNullException(EmployeeComponent employeeComponent)
        {
            if (employeeComponent is null)
            {
                throw new ArgumentNullException(nameof(employeeComponent));
            }
        }
    }
}
