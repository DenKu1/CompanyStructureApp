using CompanyStructureApp.Domain.Core.Abstract;

namespace CompanyStructureApp.Domain.Core.Interfaces
{
    public interface IEmployeeComposite
    {
        //видалений не потрібний паблик та абстракт
        void Add(EmployeeComponent employeeComponent);
        //видалений не потрібний паблик та абстракт
        void Remove(EmployeeComponent employeeComponent);
    }
}
