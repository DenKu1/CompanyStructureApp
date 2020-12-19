using CompanyStructureApp.Domain.Core.Interfaces;
using CompanyStructureApp.Domain.Core.Interfaces.Visitors;
using System.Collections;
using System.Collections.Generic;

namespace CompanyStructureApp.Domain.Core.Abstract
{
    public abstract class EmployeeComponent : IEmployeeVisitable, IEnumerable<EmployeeComponent>
    {
        public IEmployee Employee { get; private set; }

        public EmployeeComponent(IEmployee employee)
        {
            Employee = employee ?? throw null;         
        }

        public abstract void Accept(IEmployeeVisitor visitor);
        public abstract IEnumerator<EmployeeComponent> GetEnumerator();

        public string DisplayInfo()
        {
            return $"employee {Employee.Name} {Employee.Surname} with postion {Employee.Position}";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            //видалено зайвий зис
            return GetEnumerator();
        }
    }
}
