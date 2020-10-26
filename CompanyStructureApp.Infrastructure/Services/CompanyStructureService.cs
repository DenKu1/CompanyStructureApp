using AutoMapper;
using CompanyStructureApp.Domain.Core.Concrete;
using CompanyStructureApp.Domain.Interfaces.Repositories;
using CompanyStructureApp.Domain.Interfaces.Services;
using CompanyStructureApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyStructureApp.Infrastructure.Services
{
    public class CompanyStructureService : ICompanyStructureService
    {
        protected readonly ICompanyStructureRepository _repo;
        protected readonly IMapper _mp;

        public CompanyStructureService(IMapper mapper, ICompanyStructureRepository repository)
        {
            _repo = repository;
            _mp = mapper;
        }

        public string AddEmployee(EmployeeDTO employeeDTO, string superiorEmployeeId)
        {
            if (employeeDTO is null)
            {
                throw new ArgumentNullException(nameof(employeeDTO));
            }

            Employee employee = _mp.Map<Employee>(employeeDTO);

            return _repo.AddEmployee(employee, superiorEmployeeId);
        }

        public List<EmployeeDTO> ShowCompanyStructureByDirectSubordination()
        {
            var employees = _repo.GetAllEmployees();

            return _mp.Map<List<EmployeeDTO>>(employees);
        }

        public List<EmployeeDTO> ShowCompanyStructureByPositionHeight()
        {
            var employees = _repo.GetAllEmployees().OrderByDescending(e => e.Position);

            return _mp.Map<List<EmployeeDTO>>(employees);
        }
    }
}
