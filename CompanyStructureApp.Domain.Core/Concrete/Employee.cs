using CompanyStructureApp.Domain.Core.Interfaces;
using CompanyStructureApp.Settings;
using System;

namespace CompanyStructureApp.Domain.Core.Concrete
{
    public class Employee : IEmployee
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Salary { get; set; }
        public Position Position { get; set; }
        public Guid Id { get; set; }
    }
}
