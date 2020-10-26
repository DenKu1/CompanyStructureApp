using CompanyStructureApp.Domain.Interfaces.Services;
using System;


namespace CompanyStructureApp.CLI.ConsoleApp
{
    partial class ConsoleApplication
    {
        private readonly ICompanyStructureService _companyStructureService;
        private readonly IEmployeeService _employeeService;

        public ConsoleApplication(ICompanyStructureService companyStructureService, IEmployeeService employeeService)
        {
            _companyStructureService = companyStructureService;
            _employeeService = employeeService;
        }

        public void Run()
        {
            DisplayGettingStarted();
            DisplayProgramInfo();

            while (true)
            {
                Console.WriteLine("Waiting command:");
                string userInput = Console.ReadLine();

                if (userInput == "q")
                {
                    break;
                }

                try
                {
                    ProcessNextUserCommand(userInput);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine($"Incorrect data provided: {e.Message}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unknown error: {e.Message}");
                }
            }
        }
              

        void ProcessNextUserCommand(string userInput)
        {
            string[] splittedInput = userInput.Trim().Split(' ');

            if (!int.TryParse(splittedInput[0], out int option))
            {
                Console.WriteLine("Unable to parse option number!");
            }

            string message;

            switch (option)
            {
                case 1:
                    if (!EnoughParameters(5, splittedInput)) break;

                    message = AddEmployeeOption(splittedInput);

                    Console.WriteLine(message);
                    break;

                case 2:                    

                    message = ShowCompanyStructureByDirectSubordination();

                    Console.WriteLine(message);
                    break;

                case 3:
                    message = ShowCompanyStructureByPositionHeight();

                    Console.WriteLine(message);
                    break;

                case 4:
                    if (!EnoughParameters(2, splittedInput)) break;

                    message = FindEmployeesWithSalaryBiggerOption(splittedInput);

                    Console.WriteLine(message);
                    break;

                case 5:
                    message = FindEmployeesWithBiggestSalaryOption();

                    Console.WriteLine(message);
                    break;

                case 6:
                    if (!EnoughParameters(2, splittedInput)) break;

                    message = FindEmployeesWithSuperiorOption(splittedInput);

                    Console.WriteLine(message);
                    break;

                case 7:
                    if (!EnoughParameters(2, splittedInput)) break;

                    message = FindEmployeesOnPositionOption(splittedInput);

                    Console.WriteLine(message);
                    break;

                default:
                    Console.WriteLine("Unknown option!");
                    break;
            }
        }
    }
}
