using CompanyStructureApp.Domain.Core.Concrete;

namespace CompanyStructureApp.Domain.Core.Interfaces.Visitors
{
    public interface IEmployeeVisitor
    {
        void VisitEmployeeComposite(EmployeeComposite employeeComposite);

        void VisitEmployeeLeaf(EmployeeLeaf employeeLeaf);
    }
}
