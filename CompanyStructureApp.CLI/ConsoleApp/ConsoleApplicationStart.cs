using System;

namespace CompanyStructureApp.CLI.ConsoleApp
{
    partial class ConsoleApplication
    {
        private void DisplayGettingStarted()
        {
            Console.WriteLine("Welcome to the 'Company Structure Application!'");
            Console.WriteLine("Press any key to start...");
            Console.WriteLine("Enter 'q' to exit");
            Console.ReadKey();
        }

        private void DisplayProgramInfo()
        {
            Console.WriteLine("\n=======================================\n");
            Console.WriteLine("Available options:");
            Console.WriteLine("1. Add employee");
            Console.WriteLine("Provide: name, surname, salary, position, direct superior id");
            Console.WriteLine("Example: '1 Denys Kulyk 5000 director'\n");
            Console.WriteLine("2. Show company structure by direct subordination");
            Console.WriteLine("3. Show company structure by the height of the position in the company\n");
            Console.WriteLine("4. Show employees who has salary more than the value");
            Console.WriteLine("Provide: salary value");
            Console.WriteLine("5. Show employees who has the biggest salary in the company");
            Console.WriteLine("6. Show employees whose direct superior is the employee");
            Console.WriteLine("Provide: employee id");
            Console.WriteLine("7. Show employees who are on the position");
            Console.WriteLine("Provide: position name");
            Console.WriteLine("\n========================================\n");
            Console.WriteLine("To choose option please type option number and provide additional value if needed:");
        }
    }
}
