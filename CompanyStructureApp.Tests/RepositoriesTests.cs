﻿using CompanyStructureApp.Domain.Core.Abstract;
using CompanyStructureApp.Domain.Core.Concrete;
using CompanyStructureApp.Domain.Core.Interfaces;
using CompanyStructureApp.Domain.Interfaces.Repositories;
using CompanyStructureApp.Infrastructure.DataAccess;
using ExposedObject;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Dynamic;
using System.Linq;
using FluentAssertions;
using CompanyStructureApp.Domain.Core.Exceptions;
using CompanyStructureApp.Settings;

namespace CompanyStructureApp.Tests
{
    public class RepositoriesTests
    {
        private CompanyStructureRepository companyStructureRepository;

        private Mock<IEmployeeComponentFactory> mockEmployeeComponentFactory;
        private Mock<ICompanyStructureContainer> mockCompanyStructureContainer;

        #region data

        private Employee Employee => new Employee
        {
            Id = Guid.Parse("4596490f-524f-4af2-bf72-16f15bd78831"),
            Name = "Denys",
            Surname = "Kulyk",
            Position = Position.Director,
            Salary = 300
        };

        private EmployeeComposite EmployeeComposite => new EmployeeComposite(new Employee
        {
            Id = Guid.Parse("4596490f-524f-4af2-bf72-16f15bd78831"),
            Name = "Denys",
            Surname = "Kulyk",
            Position = Position.Director,
            Salary = 300
        });

        private List<EmployeeComponent> EmployeeComponentList => new List<EmployeeComponent>
        {
            new EmployeeComposite(new Employee
            {
                Id = Guid.Parse("f571936a-fa52-4dbb-a4fd-c80e28099de4"),
                Name = "Denys",
                Surname = "Kulyk",
                Position = Position.Director,
                Salary = 300
            }),
            new EmployeeComposite(new Employee
            {
                Id = Guid.Parse("7114cfaf-a0f6-4256-ba9c-98151805b638"),
                Name = "Mark",
                Surname = "Red",
                Position = Position.Manager,
                Salary = 200
            }),
            new EmployeeLeaf(new Employee
            {
                Id = Guid.Parse("8ca66163-d427-42cd-8fe0-2f16d5131db4"),
                Name = "Susan",
                Surname = "Green",
                Position = Position.Worker,
                Salary = 100
            }),
            new EmployeeComposite(new Employee
            {
                Id = Guid.Parse("14951257-4901-44e6-af04-59afb0714cb6"),
                Name = "Petro",
                Surname = "Manager",
                Position = Position.Manager,
                Salary = 100
            }),
            new EmployeeLeaf(new Employee
            {
                Id = Guid.Parse("ec6f17ca-7eb5-4587-a49d-b8c5b5a9ff3a"),
                Name = "Natalie",
                Surname = "Cyan",
                Position = Position.Worker,
                Salary = 50
            })
        };
        #endregion


        private Mock<IEmployeeComponentFactory> CreateMockFactory()
        {
            var mockFactory = new Mock<IEmployeeComponentFactory>();

            mockFactory.Setup(f => f.CreateEmployeeComponent(
                It.IsAny<IEmployee>())).Returns((IEmployee e) => new EmployeeComposite(e));

            return mockFactory;
        }

        private Mock<ICompanyStructureContainer> CreateMockContainer()
        {
            var mockContainer = new Mock<ICompanyStructureContainer>();

            mockContainer.SetupGet(f => f.RootElement).Returns(EmployeeComposite);
            mockContainer.SetupSet(f => f.RootElement = It.IsAny<EmployeeComponent>());

            mockContainer.Setup(f => f.GetEnumerator()).Returns(EmployeeComponentList.GetEnumerator());

            return mockContainer;
        }

        public RepositoriesTests()
        {
            mockCompanyStructureContainer = CreateMockContainer();
            mockEmployeeComponentFactory = CreateMockFactory();

            companyStructureRepository = new CompanyStructureRepository(
                mockCompanyStructureContainer.Object, mockEmployeeComponentFactory.Object);
        }

        [Fact]
        public void CompanyStructureRepository_AddEmployee_NullEmployeeDtoShouldRaiseException()
        {
            // Arrange
            Employee employee = null;
            string superiorEmployeeId = "some string";

            // Act
            Action act = () => companyStructureRepository.AddEmployee(employee, superiorEmployeeId);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal(nameof(employee), exception.ParamName);
        }

        [Fact]
        public void CompanyStructureRepository_AddEmployee_NullSuperiorIdShouldAddElementAsRoot()
        {
            // Arrange
            IEmployee employee = Employee;
            string superiorEmployeeId = null;

            EmployeeComponent expectedEmployeeComponent = EmployeeComposite;

            // Act
            companyStructureRepository.AddEmployee(employee, superiorEmployeeId);

            // Assert
            mockCompanyStructureContainer.VerifySet(mock => mock.RootElement =
                It.Is<EmployeeComponent>(c => c.PublicInstancePropertiesEqual(expectedEmployeeComponent)));
        }

        [Fact]
        public void CompanyStructureRepository_AddEmployee_NullSuperiorIdShouldReturnEmployeeId()
        {
            // Arrange
            IEmployee employee = Employee;
            string superiorEmployeeId = null;

            string expectedId = employee.Id.ToString();

            // Act
            var result = companyStructureRepository.AddEmployee(employee, superiorEmployeeId);

            // Assert
            Assert.Equal(expectedId, result);
        }

        [Fact]
        public void CompanyStructureRepository_AddEmployee_WrongIdShouldRaiseException()
        {
            // Arrange
            IEmployee employee = Employee;
            string superiorEmployeeId = "wrong id";

            // Act
            Action act = () => companyStructureRepository.AddEmployee(employee, superiorEmployeeId);

            // Assert
            Assert.Throws<EmployeeException>(act);
        }

        [Fact]
        public void CompanyStructureRepository_AddEmployee_SuperiorLeafRaiseException()
        {
            // Arrange
            IEmployee employee = Employee;
            string wrongSuperiorEmployeeId = "ec6f17ca-7eb5-4587-a49d-b8c5b5a9ff3a";

            // Act
            Action act = () => companyStructureRepository.AddEmployee(employee, wrongSuperiorEmployeeId);

            // Assert
            Assert.Throws<EmployeeException>(act);
        }


        [Fact]
        public void CompanyStructureRepository_AddEmployee_ShouldReturnEmployeeId()
        {
            // Arrange
            IEmployee employee = Employee;
            employee.Position = Position.Worker;
            string superiorEmployeeId = "f571936a-fa52-4dbb-a4fd-c80e28099de4";

            string expectedId = employee.Id.ToString();

            // Act
            var result = companyStructureRepository.AddEmployee(employee, superiorEmployeeId);

            // Assert
            Assert.Equal(expectedId, result);
        }

        [Fact]
        public void CompanyStructureRepository_GetEmployeeComponentById_NullIdShouldRaiseException()
        {
            // Arrange
            var exposedRepository = Exposed.From(companyStructureRepository);

            string id = null;

            // Act
            Action act = () => exposedRepository.GetEmployeeComponentById(id);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal(nameof(id), exception.ParamName);
        }

        [Fact]      
        public void CompanyStructureRepository_GetEmployeeComponentById_UnexistingItemShouldReturnNull()
        {
            // Arrange
            var exposedRepository = Exposed.From(companyStructureRepository);

            var id = "wrong id";

            var list = EmployeeComponentList;
            var expected = list.Where(e => e.Employee.Id.ToString() == id).FirstOrDefault();
            // Act
            var result = exposedRepository.GetEmployeeComponentById(id);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("8ca66163-d427-42cd-8fe0-2f16d5131db4")]
        [InlineData("14951257-4901-44e6-af04-59afb0714cb6")]        
        public void CompanyStructureRepository_GetEmployeeComponentById_ExistingItemShouldBeFound(string id)
        {
            // Arrange
            var exposedRepository = Exposed.From(companyStructureRepository);

            var list = EmployeeComponentList;
            var expected = list.Where(e => e.Employee.Id.ToString() == id).FirstOrDefault();
            // Act
            var result = exposedRepository.GetEmployeeComponentById(id);

            // Assert
            EmployeeComponent component = Assert.IsAssignableFrom<EmployeeComponent>(result);

            component.Employee.Should().BeEquivalentTo(expected.Employee);
        }

        [Fact]
        public void CompanyStructureRepository_GetEmployeeById_NullIdShouldRaiseException()
        {
            // Arrange
            string id = null;

            // Act
            Action act = () => companyStructureRepository.GetEmployeeById(id);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal(nameof(id), exception.ParamName);
        }
    }
}
