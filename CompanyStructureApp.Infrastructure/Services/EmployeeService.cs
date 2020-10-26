using AutoMapper;
using CompanyStructureApp.Domain.Interfaces.Repositories;
using CompanyStructureApp.Domain.Interfaces.Services;
using CompanyStructureApp.Settings;
using CompanyStructureApp.DTOs;
using System.Collections.Generic;
using System;

namespace CompanyStructureApp.Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        protected readonly ICompanyStructureRepository _repo;
        protected readonly IMapper _mp;

        public EmployeeService(IMapper mapper, ICompanyStructureRepository repository)
        {
            _repo = repository;
            _mp = mapper;
        }

        public List<EmployeeDTO> FindEmployeesOnPosition(Position position)
        {
            var employees = _repo.GetEmployees(ec => ec.Employee.Position == position);

            return _mp.Map<List<EmployeeDTO>>(employees);
        }

        public List<EmployeeDTO> FindEmployeesWithSalaryBigger(int salary)
        {
            var employees = _repo.GetEmployees(ec => ec.Employee.Salary > salary);

            return _mp.Map<List<EmployeeDTO>>(employees);
        }

        public List<EmployeeDTO> FindEmployeesWithBiggestSalary()
        {
            var employees = _repo.FindEmployeesWithBiggestSalary();

            return _mp.Map<List<EmployeeDTO>>(employees);
        }

        public List<EmployeeDTO> FindEmployeesWithSuperior(string superiorId)
        {
            if (superiorId is null)
            {
                throw new ArgumentNullException(nameof(superiorId));
            }

            var employees = _repo.FindEmployeesWithSuperior(superiorId);

            return _mp.Map<List<EmployeeDTO>>(employees);
        }

        
    }
}
