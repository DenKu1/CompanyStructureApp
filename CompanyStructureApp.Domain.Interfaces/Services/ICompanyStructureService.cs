using CompanyStructureApp.DTOs;
using System.Collections.Generic;

namespace CompanyStructureApp.Domain.Interfaces.Services
{
    public interface ICompanyStructureService
    {
        string AddEmployee(EmployeeDTO employeeDTO, string superiorEmployeeId);
        List<EmployeeDTO> ShowCompanyStructureByDirectSubordination();
        List<EmployeeDTO> ShowCompanyStructureByPositionHeight();
    }
}
