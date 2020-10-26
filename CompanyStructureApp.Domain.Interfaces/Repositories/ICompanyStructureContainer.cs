using CompanyStructureApp.Domain.Core.Abstract;
using System.Collections.Generic;

namespace CompanyStructureApp.Domain.Interfaces.Repositories
{
    public interface ICompanyStructureContainer : IEnumerable<EmployeeComponent>
    {
        EmployeeComponent RootElement { get; set; }
    }
}
