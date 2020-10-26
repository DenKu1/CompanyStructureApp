using CompanyStructureApp.Settings;
using System;

namespace CompanyStructureApp.DTOs
{
    public class EmployeeDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Salary { get; set; }
        public Position Position { get; set; }
        public Guid Id { get; set; }
    }
}
