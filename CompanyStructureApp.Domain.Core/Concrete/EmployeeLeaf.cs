using CompanyStructureApp.Domain.Core.Abstract;
using CompanyStructureApp.Domain.Core.Interfaces;
using System.Collections.Generic;
using System;
using CompanyStructureApp.Domain.Core.Interfaces.Visitors;

namespace CompanyStructureApp.Domain.Core.Concrete
{
    public class EmployeeLeaf : EmployeeComponent
    {
        public EmployeeLeaf(IEmployee employee) : base(employee)
        {
        }

        public override void Accept(IEmployeeVisitor visitor)
        {
            if (visitor is null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            visitor.VisitEmployeeLeaf(this);
        }

        public override IEnumerator<EmployeeComponent> GetEnumerator()
        {
            yield return this;
        }
    }
}
