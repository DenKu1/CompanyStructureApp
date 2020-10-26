using AutoMapper;
using CompanyStructureApp.Domain.Core.Abstract;
using CompanyStructureApp.Domain.Core.Concrete;
using CompanyStructureApp.Domain.Core.Interfaces;
using CompanyStructureApp.Domain.Interfaces.Repositories;
using CompanyStructureApp.DTOs;
using CompanyStructureApp.Infrastructure.Services;
using CompanyStructureApp.Settings;
using FluentAssertions;
using FluentAssertions.Collections;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace CompanyStructureApp.Tests
{
    public class ServicesTests
    {
        private CompanyStructureService companyStructureService;
        private EmployeeService employeeService;

        private Mock<ICompanyStructureRepository> mockCompanyStructureRepository;
        private IMapper mapper;

        

        private Mock<ICompanyStructureRepository> CreateMockRepository()
        {
            var mockRepository = new Mock<ICompanyStructureRepository>();

            var emp = Creators.CreateEmployee();
            var empList = Creators.CreateEmployeeList();

            mockRepository.Setup(r => r.AddEmployee(It.IsAny<IEmployee>(), It.IsAny<string>())).Returns((IEmployee e, string s) => e.Id.ToString());
            mockRepository.Setup(r => r.GetEmployeeById(It.IsAny<string>())).Returns(emp);            
            mockRepository.Setup(r => r.GetEmployees(It.IsAny<Func<EmployeeComponent, bool>>())).Returns(empList);
            mockRepository.Setup(r => r.GetAllEmployees()).Returns(empList);
            mockRepository.Setup(r => r.FindEmployeesWithBiggestSalary()).Returns(empList);
            mockRepository.Setup(r => r.FindEmployeesWithSuperior(It.IsAny<string>())).Returns(empList);

            return mockRepository;
        }

        public ServicesTests()
        {
            var mapperConfig = new MapperConfiguration(
                   opts => opts.CreateMap<EmployeeDTO, Employee>().ReverseMap());

            mapper = mapperConfig.CreateMapper();
            mockCompanyStructureRepository = CreateMockRepository();

            companyStructureService = new CompanyStructureService(mapper, mockCompanyStructureRepository.Object);
            employeeService = new EmployeeService(mapper, mockCompanyStructureRepository.Object);
        }
        
        [Fact]
        public void CompanyStructureService_AddEmployee_NullEmployeeDtoShouldRaiseException()
        {
            // Arrange
            EmployeeDTO employeeDTO = null;
            string superiorEmployeeId = "some string";

            // Act
            Action act = () => companyStructureService.AddEmployee(employeeDTO, superiorEmployeeId);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);            
            Assert.Equal(nameof(employeeDTO), exception.ParamName);
        }
        
        [Fact]
        public void CompanyStructureService_AddEmployee_ShouldReturnEmployeeDtoId()
        {
            // Arrange
            EmployeeDTO employeeDTO = Creators.CreateEmployeeDTO();
            string superiorEmployeeId = "some string";
            string expectedEmployeeId = employeeDTO.Id.ToString();
        
            // Act
            var result = companyStructureService.AddEmployee(employeeDTO, superiorEmployeeId);

            // Assert
            Assert.Equal(expectedEmployeeId, result);
        }

        [Fact]
        public void CompanyStructureService_AddEmployee_RepositoryAddUserShouldBeCalled()
        {
            // Arrange
            EmployeeDTO employeeDTO = Creators.CreateEmployeeDTO();
            string superiorEmployeeId = "some string";

            Employee employee = mapper.Map<Employee>(employeeDTO);

            // Act
            var result = companyStructureService.AddEmployee(employeeDTO, superiorEmployeeId);

            // Assert
            mockCompanyStructureRepository.Verify(mock => mock.AddEmployee(
                    It.Is<Employee>(e => e.PublicInstancePropertiesEqual(employee)),
                    It.Is<string>(s => s == superiorEmployeeId)), Times.Once);
        }

        [Fact]
        public void CompanyStructureService_ShowCompanyStructureByDirectSubordination_RepositoryGetAllEmployeesShouldBeCalled()
        {
            // Arrange           

            // Act
            companyStructureService.ShowCompanyStructureByDirectSubordination();

            // Assert
            mockCompanyStructureRepository.Verify(mock => mock.GetAllEmployees(), Times.Once);
        }

        [Fact]
        public void CompanyStructureService_ShowCompanyStructureByDirectSubordination_ShouldReturnEmployessInRightOrder()
        {
            // Arrange
            var employeesInRightOrder = Creators.CreateEmployeeList();
            var employeeDTOsInRightOrder = mapper.Map<List<EmployeeDTO>>(employeesInRightOrder);

            // Act
            var result = companyStructureService.ShowCompanyStructureByDirectSubordination();

            // Assert
            result.Should().BeEquivalentTo(employeeDTOsInRightOrder);
        }

        [Fact]
        public void CompanyStructureService_ShowCompanyStructureByPositionHeight_RepositoryGetAllEmployeesShouldBeCalled()
        {
            // Arrange           

            // Act
            companyStructureService.ShowCompanyStructureByPositionHeight();

            // Assert
            mockCompanyStructureRepository.Verify(mock => mock.GetAllEmployees(), Times.Once);
        }

        [Fact]
        public void CompanyStructureService_ShowCompanyStructureByPositionHeight_ShouldReturnEmployessInRightOrder()
        {
            // Arrange
            var employeesInRightOrder = Creators.CreateEmployeeList().OrderByDescending(e => e.Position);

            var employeeDTOsInRightOrder = mapper.Map<List<EmployeeDTO>>(employeesInRightOrder);

            // Act
            var result = companyStructureService.ShowCompanyStructureByPositionHeight();

            // Assert
            result.Should().BeEquivalentTo(employeeDTOsInRightOrder, options => options.WithStrictOrdering());
        }

        [Fact]
        public void EmployeeService_FindEmployeesWithSalaryBigger_ShouldReturnEmployees()
        {
            // Arrange
            var salary = 200;
            var expectedEmployees = Creators.CreateEmployeeList();

            // Act
            var result = employeeService.FindEmployeesWithSalaryBigger(salary);

            // Assert
            result.Should().BeEquivalentTo(expectedEmployees, options => options.WithStrictOrdering());
        }

        [Fact]
        public void EmployeeService_FindEmployeesWithSalaryBigger_RepositoryGetEmployeesShouldBeCalled()
        {
            // Arrange
            var salary = 200;
            
            // Act
            employeeService.FindEmployeesWithSalaryBigger(salary);

            // Assert
            mockCompanyStructureRepository.Verify(mock => mock.GetEmployees(It.IsAny<Func<EmployeeComponent, bool>>()), Times.Once);
        }

        //
        [Fact]
        public void EmployeeService_FindEmployeesWithBiggestSalary_ShouldReturnEmployees()
        {
            // Arrange
            var expectedEmployees = Creators.CreateEmployeeList();

            // Act
            var result = employeeService.FindEmployeesWithBiggestSalary();

            // Assert
            result.Should().BeEquivalentTo(expectedEmployees, options => options.WithStrictOrdering());
        }

        [Fact]
        public void EmployeeService_FindEmployeesWithBiggestSalary_RepositoryFindEmployeesWithBiggestSalaryShouldBeCalled()
        {
            // Arrange

            // Act
            employeeService.FindEmployeesWithBiggestSalary();

            // Assert
            mockCompanyStructureRepository.Verify(mock => mock.FindEmployeesWithBiggestSalary(), Times.Once);
        }

        //
        [Fact]
        public void EmployeeService_FindEmployeesOnPosition_ShouldReturnEmployees()
        {
            // Arrange
            var position = Position.Director;
            var expectedEmployees = Creators.CreateEmployeeList();

            // Act
            var result = employeeService.FindEmployeesOnPosition(position);

            // Assert
            result.Should().BeEquivalentTo(expectedEmployees, options => options.WithStrictOrdering());
        }

        [Fact]
        public void EmployeeService_FindEmployeesOnPosition_RepositoryGetEmployeesShouldBeCalled()
        {
            // Arrange
            var position = Position.Director;

            // Act
            employeeService.FindEmployeesOnPosition(position);

            // Assert
            mockCompanyStructureRepository.Verify(mock => mock.GetEmployees(It.IsAny<Func<EmployeeComponent, bool>>()), Times.Once);
        }

        [Fact]
        public void EmployeeService_FindEmployeesWithSuperior_NullIdShouldRaiseException()
        {
            // Arrange
            string superiorId = null;

            // Act
            Action act = () => employeeService.FindEmployeesWithSuperior(superiorId);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal(nameof(superiorId), exception.ParamName);
        }

        [Fact]
        public void EmployeeService_FindEmployeesWithSuperior_ShouldReturnEmployees()
        {
            // Arrange
            var superiorId = "some string";
            var expectedEmployees = Creators.CreateEmployeeList();

            // Act
            var result = employeeService.FindEmployeesWithSuperior(superiorId);

            // Assert
            result.Should().BeEquivalentTo(expectedEmployees, options => options.WithStrictOrdering());
        }

        [Fact]
        public void EmployeeService_FindEmployeesWithSuperior_RepositoryFindEmployeesWithSuperiorShouldBeCalled()
        {
            // Arrange
            var superiorId = "some string";

            // Act
            employeeService.FindEmployeesWithSuperior(superiorId);

            // Assert
            mockCompanyStructureRepository.Verify(mock => mock.FindEmployeesWithSuperior(It.Is<string>(s => s == superiorId)), Times.Once);
        }


    }
}
