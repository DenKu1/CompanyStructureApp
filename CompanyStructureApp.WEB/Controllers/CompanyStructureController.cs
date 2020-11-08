using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CompanyStructureApp.Domain.Interfaces.Services;
using CompanyStructureApp.DTOs;
using CompanyStructureApp.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CompanyStructureApp.WEB.Controllers
{    
    public class CompanyStructureController : Controller
    {
        private readonly IMapper _mapper;

        private readonly ICompanyStructureService _companyStructureService;
        private readonly IEmployeeService _employeeService;

        public CompanyStructureController(IMapper mapper, ICompanyStructureService companyStructureService, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _companyStructureService = companyStructureService;
            _employeeService = employeeService;
        }

        [HttpGet]
        public IActionResult Index(string order)
        {
            List<EmployeeDTO> empoyeeDTOs;

            switch (order) 
            {
                case "directsubordination":
                    empoyeeDTOs = _companyStructureService
                        .ShowCompanyStructureByDirectSubordination();
                    break;
                case "positionheight":
                    empoyeeDTOs = _companyStructureService
                        .ShowCompanyStructureByPositionHeight();
                    break;
                default:
                    empoyeeDTOs = _companyStructureService
                        .ShowCompanyStructureByDirectSubordination();
                    break;
            }
            
            var employeeVMs = _mapper.Map<List<EmployeeVM>>(empoyeeDTOs);

            return View(employeeVMs);
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeVM employeeVM)
        {
            if (!ModelState.IsValid)
            {
                return View(employeeVM);
            }

            employeeVM.Id = Guid.NewGuid();

            var employeeDTO = _mapper.Map<EmployeeDTO>(employeeVM);

            try
            {
                _companyStructureService.AddEmployee(employeeDTO, employeeVM.SuperiorId != null ? employeeVM.SuperiorId.ToString() : null);
            }
            catch(Exception e) 
            {
                ModelState.AddModelError("", e.Message);
                return View(employeeVM);
            }

            return RedirectToAction("Index");
        }
    }
}
