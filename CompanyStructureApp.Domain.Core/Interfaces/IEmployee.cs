using CompanyStructureApp.Settings;
using System;

namespace CompanyStructureApp.Domain.Core.Interfaces
{
    public interface IEmployee
    {
        string Name { get; set; }
        string Surname { get; set; }
        int Salary { get; set; }
        Position Position { get; set; }
        Guid Id { get; set; }
    }
}
