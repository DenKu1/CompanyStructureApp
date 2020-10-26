using System;

namespace CompanyStructureApp.CLI.ConsoleApp
{
    partial class ConsoleApplication
    {
        private bool EnoughParameters(int parametersNeeded, string[] parameters)
        {
            if (parameters.Length < parametersNeeded)
            {
                Console.WriteLine("All parameters must be provided");
                return false;
            }

            return true;
        }
    }
}
