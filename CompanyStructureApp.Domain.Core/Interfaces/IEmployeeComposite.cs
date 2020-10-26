using CompanyStructureApp.Domain.Core.Abstract;

namespace CompanyStructureApp.Domain.Core.Interfaces
{
    public interface IEmployeeComposite
    {
        public abstract void Add(EmployeeComponent employeeComponent);

        public abstract void Remove(EmployeeComponent employeeComponent);
    }
}
