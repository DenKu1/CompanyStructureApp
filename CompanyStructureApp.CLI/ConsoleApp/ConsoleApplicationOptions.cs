using CompanyStructureApp.DTOs;
using CompanyStructureApp.Settings;
using System;
using System.Text;

namespace CompanyStructureApp.CLI.ConsoleApp
{
    partial class ConsoleApplication
    {
        private string AddEmployeeOption(string[] parameters)
        {
            var employeeDTO = new EmployeeDTO()
            {
                Name = parameters[1],
                Surname = parameters[2],
                Salary = int.Parse(parameters[3]),
                Position = (Position)Enum.Parse(typeof(Position), parameters[4]),
                Id = Guid.NewGuid()
            };

            string employeeId = parameters.Length < 6
                ? _companyStructureService.AddEmployee(employeeDTO, null)
                : _companyStructureService.AddEmployee(employeeDTO, parameters[5]);

            return $"New employee was added with id:\n{employeeId}";
        }

        private string ShowCompanyStructureByDirectSubordination()
        {
            var employeeDTOs = _companyStructureService.ShowCompanyStructureByDirectSubordination();

            StringBuilder sb = new StringBuilder($"Company structure by direct subordination:\n");
            employeeDTOs.ForEach(e => sb.Append($"|{new String('-', (int)e.Position)}>{e.Position}: {e.Name} {e.Surname} {e.Id}\n"));

            return sb.ToString();
        }        

        private string ShowCompanyStructureByPositionHeight()
        {
            var employeeDTOs = _companyStructureService.ShowCompanyStructureByPositionHeight();

            StringBuilder sb = new StringBuilder($"Company structure by direct subordination:\n");
            employeeDTOs.ForEach(e => sb.Append($"|{new String('-', (int)e.Position)}>{e.Position}: {e.Name} {e.Surname} {e.Id}\n"));

            return sb.ToString();
        }


        private string FindEmployeesWithSalaryBiggerOption(string[] parameters)
        {
            int salary = int.Parse(parameters[1]);

            var employeeDTOs = _employeeService.FindEmployeesWithSalaryBigger(salary);

            StringBuilder sb = new StringBuilder($"Employees with salary bigger than {parameters[1]}:\n");
            employeeDTOs.ForEach(e => sb.Append($"{e.Name} {e.Surname} {e.Salary} {e.Id}\n"));

            return sb.ToString();
        }

        private string FindEmployeesWithBiggestSalaryOption(string[] parameters = null)
        {
            var employeeDTOs = _employeeService.FindEmployeesWithBiggestSalary();

            StringBuilder sb = new StringBuilder($"Employees with the biggest salary in the company:\n");
            employeeDTOs.ForEach(e => sb.Append($"{e.Name} {e.Surname} {e.Salary} {e.Id}\n"));

            return sb.ToString();
        }

        private string FindEmployeesWithSuperiorOption(string[] parameters)
        {
            var employeeDTOs = _employeeService.FindEmployeesWithSuperior(parameters[1]);

            StringBuilder sb = new StringBuilder($"Employees with this superior:\n");
            employeeDTOs.ForEach(e => sb.Append($"{e.Name} {e.Surname} {e.Salary} {e.Id}\n"));

            return sb.ToString();
        }

        private string FindEmployeesOnPositionOption(string[] parameters)
        {
            Position position = (Position)Enum.Parse(typeof(Position), parameters[1]);

            var employeeDTOs = _employeeService.FindEmployeesOnPosition(position);

            StringBuilder sb = new StringBuilder($"Employees on the position {position}:\n");
            employeeDTOs.ForEach(e => sb.Append($"{e.Name} {e.Surname} {e.Salary} {e.Id}\n"));

            return sb.ToString();
        }      

    }
}
