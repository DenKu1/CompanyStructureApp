using CompanyStructureApp.DTOs;
using CompanyStructureApp.Settings;
using System.Collections.Generic;

namespace CompanyStructureApp.Domain.Interfaces.Services
{
    public interface IEmployeeService
    {
        List<EmployeeDTO> FindEmployeesWithBiggestSalary();
        List<EmployeeDTO> FindEmployeesWithSalaryBigger(int salary);
        List<EmployeeDTO> FindEmployeesWithSuperior(string superiorId);
        List<EmployeeDTO> FindEmployeesOnPosition(Position position);
        List<EmployeeDTO> FindAllEmployees();
    }
}
