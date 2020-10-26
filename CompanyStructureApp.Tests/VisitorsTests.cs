using System;
using System.Collections.Generic;
using CompanyStructureApp.Domain.Core.Abstract;
using CompanyStructureApp.Domain.Core.Concrete;
using CompanyStructureApp.Domain.Core.Concrete.Visitors;
using CompanyStructureApp.Domain.Core.Interfaces;
using CompanyStructureApp.Settings;
using ExposedObject;
using FluentAssertions;
using Xunit;

namespace CompanyStructureApp.Tests
{
    public class VisitorsTests
    {
        private EmployeeMaxSalaryVisitor employeeMaxSalaryVisitor;
        private EmployeeWithSuperiorVisitor employeeWithSuperiorVisitor;

        #region data
        private Employee SuperiorEmployee => new Employee
        {
            Id = Guid.Parse("4596490f-524f-4af2-bf72-16f15bd78831"),
            Name = "Denys",
            Surname = "Kulyk",
            Position = Position.Director,
            Salary = 300
        };

        private Employee SubordinatedEmployee1 => new Employee
        {
            Id = Guid.Parse("7114cfaf-a0f6-4256-ba9c-98151805b638"),
            Name = "Mark",
            Surname = "Red",
            Position = Position.Manager,
            Salary = 200
        };

        private Employee SubordinatedEmployee2 => new Employee
        {
            Id = Guid.Parse("8ca66163-d427-42cd-8fe0-2f16d5131db4"),
            Name = "Susan",
            Surname = "Green",
            Position = Position.Worker,
            Salary = 100
        };
        #endregion

        private EmployeeComposite CreateSuperiorComposite()
        {
            var superiorEmployee = SuperiorEmployee;

            var subordinatedEmployee1 = SubordinatedEmployee1;
            var subordinatedEmployee2 = SubordinatedEmployee2;

            var subordinatedComposite1 = new EmployeeComposite(subordinatedEmployee1);
            var subordinatedComposite2 = new EmployeeComposite(subordinatedEmployee2);

            var superiroComposite = new EmployeeComposite(superiorEmployee);

            superiroComposite.Add(subordinatedComposite1);
            superiroComposite.Add(subordinatedComposite2);

            return superiroComposite;
        }

        public VisitorsTests()
        {
            string superiorEmployeeId = SuperiorEmployee.Id.ToString();

            employeeWithSuperiorVisitor = new EmployeeWithSuperiorVisitor(superiorEmployeeId);
            employeeMaxSalaryVisitor = new EmployeeMaxSalaryVisitor();            
        }

        [Fact]
        public void EmployeeWithSuperiorVisitor_VisitEmployeeComposite_NullEmployeeCompositeShouldRaiseException()
        {
            // Arrange
            EmployeeComposite employeeComposite = null;

            // Act
            Action act = () => employeeWithSuperiorVisitor.VisitEmployeeComposite(employeeComposite);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal(nameof(employeeComposite), exception.ParamName);
        }

        [Fact]
        public void EmployeeWithSuperiorVisitor_VisitEmployeeComposite_SuperiorEmployeeCompositeShouldAddSubordinatedToList()
        {
            // Arrange
            EmployeeComposite superiorEmployee = CreateSuperiorComposite();

            List<IEmployee> expectedList = new List<IEmployee> { SubordinatedEmployee1, SubordinatedEmployee2 };

            // Act
            employeeWithSuperiorVisitor.VisitEmployeeComposite(superiorEmployee);
            var result = employeeWithSuperiorVisitor.EmployeesWithSuperior;

            // Assert
            result.Should().BeEquivalentTo(expectedList);
        }

        [Fact]
        public void EmployeeWithSuperiorVisitor_VisitEmployeeComposite_SubordinateEmployeeCompositeShouldNotChangeList()
        {
            // Arrange
            EmployeeComposite superiorEmployee = CreateSuperiorComposite();
            EmployeeComposite subordinateEmployee = new EmployeeComposite(SubordinatedEmployee1);            

            List<IEmployee> expectedList = new List<IEmployee> { SubordinatedEmployee1, SubordinatedEmployee2 };

            // Act
            employeeWithSuperiorVisitor.VisitEmployeeComposite(superiorEmployee);
            employeeWithSuperiorVisitor.VisitEmployeeComposite(subordinateEmployee);
            var result = employeeWithSuperiorVisitor.EmployeesWithSuperior;

            // Assert
            result.Should().BeEquivalentTo(expectedList);
        }

        [Fact]
        public void EmployeeWithSuperiorVisitor_VisitEmployeeLeaf_NullEmployeeCompositeShouldRaiseException()
        {
            // Arrange
            EmployeeLeaf employeeLeaf = null;

            // Act
            Action act = () => employeeWithSuperiorVisitor.VisitEmployeeLeaf(employeeLeaf);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal(nameof(employeeLeaf), exception.ParamName);
        }

        [Fact]
        public void EmployeeMaxSalaryVisitor_VisitEmployeeComponent_NullEmployeeCompositeShouldRaiseException()
        {
            // Arrange
            var exposedVisitor = Exposed.From(employeeMaxSalaryVisitor);

            EmployeeComponent employeeComponent = null;

            // Act
            Action act = () => exposedVisitor.VisitEmployeeComponent(employeeComponent);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal(nameof(employeeComponent), exception.ParamName);
        }

        [Fact]
        public void EmployeeMaxSalaryVisitor_VisitEmployeeComponent_WhenListEmptyAnyEmployeeShouldAdded()
        {
            // Arrange
            var exposedVisitor = Exposed.From(employeeMaxSalaryVisitor);

            EmployeeComponent employeeComponent = new EmployeeLeaf(SubordinatedEmployee1);

            List<IEmployee> expectedList = new List<IEmployee> { SubordinatedEmployee1 };

            // Act
            exposedVisitor.VisitEmployeeComponent(employeeComponent);
            List<IEmployee> result = exposedVisitor.EmployeesWithMaxSalary;

            // Assert
            result.Should().BeEquivalentTo(expectedList);
        }

        [Fact]
        public void EmployeeMaxSalaryVisitor_VisitEmployeeComponent_WhenListHasElementWithTheSameSalaryAsCurrentShouldAddBoth()
        {
            // Arrange
            var exposedVisitor = Exposed.From(employeeMaxSalaryVisitor);

            EmployeeComponent employeeComponent1 = new EmployeeLeaf(SubordinatedEmployee1);
            EmployeeComponent employeeComponent2 = new EmployeeLeaf(SubordinatedEmployee1);

            List<IEmployee> expectedList = new List<IEmployee> { SubordinatedEmployee1, SubordinatedEmployee1 };

            // Act
            exposedVisitor.VisitEmployeeComponent(employeeComponent1);
            exposedVisitor.VisitEmployeeComponent(employeeComponent2);
            List<IEmployee> result = exposedVisitor.EmployeesWithMaxSalary;

            // Assert
            result.Should().BeEquivalentTo(expectedList);
        }

        [Fact]
        public void EmployeeMaxSalaryVisitor_VisitEmployeeComponent_WhenListHasElementWithTheLessSalaryThanCurrentShouldAddBigger()
        {
            // Arrange
            var exposedVisitor = Exposed.From(employeeMaxSalaryVisitor);

            EmployeeComponent biggerEmployeeComponent1 = new EmployeeLeaf(SubordinatedEmployee1);
            EmployeeComponent smallerEmployeeComponent2 = new EmployeeLeaf(SubordinatedEmployee2);

            List<IEmployee> expectedList = new List<IEmployee> { SubordinatedEmployee1 };

            // Act
            exposedVisitor.VisitEmployeeComponent(smallerEmployeeComponent2);
            exposedVisitor.VisitEmployeeComponent(biggerEmployeeComponent1);

            List<IEmployee> result = exposedVisitor.EmployeesWithMaxSalary;

            // Assert
            result.Should().BeEquivalentTo(expectedList);
        }

        [Fact]
        public void EmployeeMaxSalaryVisitor_VisitEmployeeComponent_WhenListHasElementWithTheBiggerSalaryThanCurrentShouldNotAddSmaller()
        {
            // Arrange
            var exposedVisitor = Exposed.From(employeeMaxSalaryVisitor);

            EmployeeComponent biggerEmployeeComponent1 = new EmployeeLeaf(SubordinatedEmployee1);
            EmployeeComponent smallerEmployeeComponent2 = new EmployeeLeaf(SubordinatedEmployee2);

            List<IEmployee> expectedList = new List<IEmployee> { SubordinatedEmployee1 };

            // Act
            exposedVisitor.VisitEmployeeComponent(biggerEmployeeComponent1);
            exposedVisitor.VisitEmployeeComponent(smallerEmployeeComponent2);
            
            List<IEmployee> result = exposedVisitor.EmployeesWithMaxSalary;

            // Assert
            result.Should().BeEquivalentTo(expectedList);
        }
    }
}
