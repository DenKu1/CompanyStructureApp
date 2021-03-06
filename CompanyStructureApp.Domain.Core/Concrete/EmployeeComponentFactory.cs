﻿using CompanyStructureApp.Domain.Core.Abstract;
using CompanyStructureApp.Domain.Core.Interfaces;

namespace CompanyStructureApp.Domain.Core.Concrete
{
    public class EmployeeComponentFactory : IEmployeeComponentFactory
    {
        public EmployeeComponent CreateEmployeeComponent(IEmployee employee)
        {
            if (employee is null)
            {
                throw new System.ArgumentNullException(nameof(employee));
            }

            if (employee.Position != 0)
            {
                return new EmployeeComposite(employee);
            }
            else
            {
                return new EmployeeLeaf(employee);
            }
        }

    }
}
