using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using AutoMapper;
using CompanyStructureApp.Domain.Interfaces.Services;
using CompanyStructureApp.DTOs;
using CompanyStructureApp.Settings;
using CompanyStructureApp.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CompanyStructureApp.WEB.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;

        private readonly ICompanyStructureService _companyStructureService;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IMapper mapper, ICompanyStructureService companyStructureService, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _companyStructureService = companyStructureService;
            _employeeService = employeeService;
        }

        public IActionResult Index(string selector, string arg)
        {
            List<EmployeeDTO> empoyeeDTOs;

            try
            {
                switch (selector)
                {
                    case "biggestsalary":
                        empoyeeDTOs = _employeeService
                            .FindEmployeesWithBiggestSalary();
                        break;
                    case "salarybigger":
                        empoyeeDTOs = _employeeService
                            .FindEmployeesWithSalaryBigger(int.Parse(arg));
                        break;
                    case "withsuperior":
                        empoyeeDTOs = _employeeService
                            .FindEmployeesWithSuperior(arg);
                        break;
                    case "onposition":
                        empoyeeDTOs = _employeeService
                            .FindEmployeesOnPosition(Enum.Parse<Position>(arg));
                        break;
                    default:
                        empoyeeDTOs = _employeeService
                            .FindAllEmployees();
                        break;
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View();
            }

            var employeeVMs = _mapper.Map<List<EmployeeVM>>(empoyeeDTOs);

            return View(employeeVMs);
        }

    }
}
