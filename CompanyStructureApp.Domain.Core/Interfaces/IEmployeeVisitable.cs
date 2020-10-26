using CompanyStructureApp.Domain.Core.Interfaces.Visitors;

namespace CompanyStructureApp.Domain.Core.Interfaces
{
    public interface IEmployeeVisitable
    {
        void Accept(IEmployeeVisitor visitor);
    }
}
