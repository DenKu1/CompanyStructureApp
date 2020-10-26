using CompanyStructureApp.Domain.Core.Abstract;
using CompanyStructureApp.Domain.Core.Concrete;
using CompanyStructureApp.Domain.Core.Interfaces;
using CompanyStructureApp.DTOs;
using CompanyStructureApp.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyStructureApp.Tests
{
    static class Creators
    {
        public static IEmployee CreateEmployee()
        {
            return new Employee
            {
                Id = Guid.Parse("4596490f-524f-4af2-bf72-16f15bd78831"),
                Name = "Denys",
                Surname = "Kulyk",
                Position = Position.Director,
                Salary = 300
            };
        }

        public static IEmployee CreateWorker()
        {
            return new Employee
            {
                Id = Guid.Parse("74e97af3-b44e-42f1-87a0-f291e0a80cff"),
                Name = "Vova",
                Surname = "Green",
                Position = Position.Worker,
                Salary = 100
            };
        }

        public static EmployeeDTO CreateEmployeeDTO()
        {
            return new EmployeeDTO
            {
                Id = Guid.Parse("4596490f-524f-4af2-bf72-16f15bd78831"),
                Name = "Denys",
                Surname = "Kulyk",
                Position = Position.Worker,
                Salary = 300
            };
        }

        public static EmployeeComponent CreateEmployeeComponent()
        {
            return new EmployeeComposite(CreateEmployee());
        }


        public static List<IEmployee> CreateEmployeeList()
        {
            return new List<IEmployee>()
            {
                new Employee
                {
                    Id = Guid.Parse("f571936a-fa52-4dbb-a4fd-c80e28099de4"),
                    Name = "Denys",
                    Surname = "Kulyk",
                    Position = Position.Director,
                    Salary = 300
                },
                new Employee
                {
                    Id = Guid.Parse("7114cfaf-a0f6-4256-ba9c-98151805b638"),
                    Name = "Mark",
                    Surname = "Red",
                    Position = Position.Manager,
                    Salary = 200
                },
                new Employee
                {
                    Id = Guid.Parse("8ca66163-d427-42cd-8fe0-2f16d5131db4"),
                    Name = "Susan",
                    Surname = "Green",
                    Position = Position.Worker,
                    Salary = 100
                },
                new Employee
                {
                    Id = Guid.Parse("14951257-4901-44e6-af04-59afb0714cb6"),
                    Name = "Petro",
                    Surname = "Manager",
                    Position = Position.Manager,
                    Salary = 100
                },
                new Employee
                {
                    Id = Guid.Parse("ec6f17ca-7eb5-4587-a49d-b8c5b5a9ff3a"),
                    Name = "Natalie",
                    Surname = "Cyan",
                    Position = Position.Worker,
                    Salary = 50
                }

            };
        }

        public static List<EmployeeComponent> CreateEmployeeComponentList()
        {
            return CreateEmployeeList()
                .Select(e => e.Position != Position.Worker ? (EmployeeComponent)new EmployeeComposite(e) : (EmployeeComponent)new EmployeeLeaf(e))
                .ToList();
        }
    }
}
