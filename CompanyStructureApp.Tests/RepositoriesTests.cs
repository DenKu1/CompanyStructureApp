using CompanyStructureApp.Domain.Core.Abstract;
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

namespace CompanyStructureApp.Tests
{
    public class RepositoriesTests
    {
        private CompanyStructureRepository companyStructureRepository;

        private Mock<IEmployeeComponentFactory> mockEmployeeComponentFactory;
        private Mock<ICompanyStructureContainer> mockCompanyStructureContainer;

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

            mockContainer.SetupGet(f => f.RootElement).Returns(Creators.CreateEmployeeComponent());
            mockContainer.SetupSet(f => f.RootElement = It.IsAny<EmployeeComponent>());

            mockContainer.Setup(f => f.GetEnumerator()).Returns(Creators.CreateEmployeeComponentList().GetEnumerator());

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
            IEmployee employee = Creators.CreateEmployee();
            string superiorEmployeeId = null;

            EmployeeComponent expectedEmployeeComponent = Creators.CreateEmployeeComponent();

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
            IEmployee employee = Creators.CreateEmployee();
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
            IEmployee employee = Creators.CreateEmployee();
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
            IEmployee employee = Creators.CreateEmployee();
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
            IEmployee employee = Creators.CreateEmployee();
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

            var list = Creators.CreateEmployeeComponentList();
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

            var list = Creators.CreateEmployeeComponentList();
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
