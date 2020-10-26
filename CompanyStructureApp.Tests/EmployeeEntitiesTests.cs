using CompanyStructureApp.Domain.Core.Abstract;
using CompanyStructureApp.Domain.Core.Concrete;
using CompanyStructureApp.Domain.Core.Interfaces;
using CompanyStructureApp.Domain.Core.Interfaces.Visitors;
using CompanyStructureApp.Settings;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace CompanyStructureApp.Tests
{
    public class EmployeeEntitiesTests
    {
        private EmployeeLeaf employeeLeaf;
        private EmployeeComposite employeeComposite;
        private EmployeeComponentFactory employeeFactory;

        private Mock<IEmployeeVisitor> mockVisitor;

        #region data
        private Employee Employee => new Employee
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

        public EmployeeEntitiesTests()
        {
            mockVisitor = new Mock<IEmployeeVisitor>();

            employeeLeaf = new EmployeeLeaf(Employee);
            employeeComposite = new EmployeeComposite(Employee);
            employeeFactory = new EmployeeComponentFactory();
        }

        [Fact]
        public void EmployeeLeaf_Accept_NullVisitorShouldRaiseException()
        {
            // Arrange
            IEmployeeVisitor visitor = null;

            // Act
            Action act = () => employeeLeaf.Accept(visitor);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal(nameof(visitor), exception.ParamName);
        }

        [Fact]
        public void EmployeeLeaf_Accept_VisitEmployeeLeafShouldBeCalled()
        {
            // Arrange
            IEmployeeVisitor visitor = mockVisitor.Object;

            // Act
            employeeLeaf.Accept(visitor);

            // Assert
            mockVisitor.Verify(mock => mock.VisitEmployeeLeaf(It.Is<EmployeeLeaf>(e => e == employeeLeaf)), Times.Once);
        }


        [Fact]
        public void EmployeeLeaf_GetEnumerator_EnumeratorShouldReturnThis()
        {
            // Arrange            
            var enumerator = employeeLeaf.GetEnumerator();

            // Act
            enumerator.MoveNext();
            var result = enumerator.Current;

            // Assert
            Assert.True(employeeLeaf == result);
        }

        [Fact]
        public void EmployeeComposite_Accept_NullVisitorShouldRaiseException()
        {
            // Arrange
            IEmployeeVisitor visitor = null;

            // Act
            Action act = () => employeeComposite.Accept(visitor);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal(nameof(visitor), exception.ParamName);
        }

        [Fact]
        public void EmployeeComposite_Accept_VisitEmployeeCompositeShouldBeCalled()
        {
            // Arrange
            IEmployeeVisitor visitor = mockVisitor.Object;

            // Act
            employeeComposite.Accept(visitor);

            // Assert
            mockVisitor.Verify(mock => mock.VisitEmployeeComposite(It.Is<EmployeeComposite>(e => e == employeeComposite)), Times.Once);
        }

        [Fact]
        public void EmployeeComposite_Accept_AllSuboridinatesAcceptShouldBeCalled()
        {
            // Arrange
            var mockSubordinate1 = new Mock<EmployeeLeaf>(SubordinatedEmployee1);
            var mockSubordinate2 = new Mock<EmployeeLeaf>(SubordinatedEmployee2);

            employeeComposite.Add(mockSubordinate1.Object);
            employeeComposite.Add(mockSubordinate2.Object);

            IEmployeeVisitor visitor = mockVisitor.Object;

            // Act
            employeeComposite.Accept(visitor);

            // Assert
            mockSubordinate1.Verify(mock => mock.Accept(It.Is<IEmployeeVisitor>(e => e == visitor)), Times.Once);
            mockSubordinate2.Verify(mock => mock.Accept(It.Is<IEmployeeVisitor>(e => e == visitor)), Times.Once);
        }

        [Fact]
        public void EmployeeComposite_GetEnumerator_EnumeratorShouldReturnThis()
        {
            // Arrange            
            var enumerator = employeeComposite.GetEnumerator();

            // Act
            enumerator.MoveNext();
            var result = enumerator.Current;

            // Assert
            Assert.True(employeeComposite == result);
        }

        [Fact]
        public void EmployeeComposite_GetEnumerator_AllSuboridinatesShouldBeReturned()
        {
            // Arrange
            var subordinate1 = new EmployeeLeaf(SubordinatedEmployee1);
            var subordinate2 = new EmployeeLeaf(SubordinatedEmployee2);

            employeeComposite.Add(subordinate1);
            employeeComposite.Add(subordinate2);

            var enumerator = employeeComposite.GetEnumerator();
            enumerator.MoveNext();

            List<EmployeeComponent> expected = new List<EmployeeComponent> { subordinate1 , subordinate2 };
            List<EmployeeComponent> result = new List<EmployeeComponent>();

            // Act
            while (enumerator.MoveNext())
            {
                result.Add(enumerator.Current);
            }

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void EmployeeComponentFactory_CreateEmployeeComponent_NullEmployeeShouldRaiseException()
        {
            // Arrange
            IEmployee employee = null;

            // Act
            Action act = () => employeeFactory.CreateEmployeeComponent(employee);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal(nameof(employee), exception.ParamName);
        }

        [Fact]
        public void EmployeeComponentFactory_CreateEmployeeComponent_WorkerShouldReturnLeaf()
        {
            // Arrange
            var worker = Employee;
            worker.Position = Position.Worker;

            // Act
            var result = employeeFactory.CreateEmployeeComponent(worker);

            // Assert
            Assert.IsAssignableFrom<EmployeeLeaf>(result);
        }

        [Fact]
        public void EmployeeComponentFactory_CreateEmployeeComponent_NoNWorkerShouldReturnComposite()
        {
            // Arrange
            var nonWorker = Employee;
            nonWorker.Position = Position.Manager;

            // Act
            var result = employeeFactory.CreateEmployeeComponent(nonWorker);

            // Assert
            Assert.IsAssignableFrom<EmployeeComposite>(result);
        }
    }
}
