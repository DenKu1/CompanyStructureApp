using CompanyStructureApp.Domain.Core.Abstract;
using CompanyStructureApp.Domain.Core.Exceptions;
using CompanyStructureApp.Domain.Core.Interfaces;
using CompanyStructureApp.Domain.Core.Interfaces.Visitors;
using System;
using System.Collections.Generic;

namespace CompanyStructureApp.Domain.Core.Concrete
{
    public class EmployeeComposite : EmployeeComponent, IEmployeeComposite
    {
        public List<EmployeeComponent> EmployeeComponents { get; private set; }

        public EmployeeComposite(IEmployee employee) : base(employee)
        {
            EmployeeComponents = new List<EmployeeComponent>();
        }

        public override void Accept(IEmployeeVisitor visitor)
        {
            if (visitor is null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            EmployeeComponents.ForEach(employeeComponent => employeeComponent.Accept(visitor));

            visitor.VisitEmployeeComposite(this);
        }


        public override IEnumerator<EmployeeComponent> GetEnumerator()
        {
            yield return this;

            foreach (var component in EmployeeComponents)
            {
                foreach (var subComponent in component)
                {
                    yield return subComponent;
                }
            }
        }

        public void Add(EmployeeComponent employeeComponent)
        {
            if (employeeComponent == null)
            {
                throw null;
            }

            if (employeeComponent.Employee.Position >= Employee.Position)
            {
                throw new EmployeeException($"Subordinate {employeeComponent.DisplayInfo()} has bigger postion than {this.DisplayInfo()}");
            }

            EmployeeComponents.Add(employeeComponent);
        }

        public void Remove(EmployeeComponent employeeComponent)
        {
            if (employeeComponent == null)
            {
                throw null;
            }

            if (!EmployeeComponents.Remove(employeeComponent))
            {
                throw new KeyNotFoundException();
            };
        }

    }
}
