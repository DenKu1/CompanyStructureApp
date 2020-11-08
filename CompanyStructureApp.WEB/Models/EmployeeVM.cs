using CompanyStructureApp.Settings;
using System;
using System.ComponentModel.DataAnnotations;

namespace CompanyStructureApp.WEB.Models
{
    public class EmployeeVM
    {
        public Guid? Id { get; set; }

        public Guid? SuperiorId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        public int Salary { get; set; }

        public Position Position { get; set; }       
    }
}