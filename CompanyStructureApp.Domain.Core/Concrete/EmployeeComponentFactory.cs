using CompanyStructureApp.Domain.Core.Abstract;
using CompanyStructureApp.Domain.Core.Interfaces;

namespace CompanyStructureApp.Domain.Core.Concrete
{
    public class EmployeeComponentFactory : IEmployeeComponentFactory
    {
        public EmployeeComponent CreateEmployeeComponent(IEmployee employee)
        {

            if (employee.Position != 0)
            {
                return new EmployeeComposite(employee);
            }
            //видалено непотрібний елс

            return new EmployeeLeaf(employee);
        }

    }
}
