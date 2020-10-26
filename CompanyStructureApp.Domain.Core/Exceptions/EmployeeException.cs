using System;

namespace CompanyStructureApp.Domain.Core.Exceptions
{
    public class EmployeeException : Exception
    {
        public EmployeeException() : base() 
        {
        }

        public EmployeeException(string message) : base(message)
        {
        }
    }
}
