using CompanyStructureApp.Domain.Core.Abstract;

namespace CompanyStructureApp.Domain.Core.Interfaces
{
    public interface IEmployeeComponentFactory
    {
        EmployeeComponent CreateEmployeeComponent(IEmployee employee);
    }
}
