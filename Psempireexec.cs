using System;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;

namespace ActiveDirectory
{
    class Program
    {
        static void Main(string[] args)
        {
            // Declare variables to store user, password, domain, and empirePath
            string user = null;
            string password = null;
            string domain = null;
            string empirePath = null;

            // Parse command line arguments
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-u":
                        user = args[++i];
                        break;
                    case "-p":
                        password = args[++i];
                        break;
                    case "-v":
                        domain = args[++i];
                        break;
                    case "-e":
                        empirePath = args[++i];
                        break;
                }
            }

            // Check if all required arguments are provided
            if (user == null || password == null || domain == null || empirePath == null)
            {
                Console.WriteLine("Usage: -u [user] -p [password] -v [domain] -e [empire_path]");
                return;
            }

            // Connect to Active Directory using provided credentials
            using (var context = new PrincipalContext(ContextType.Domain, domain, user, password))
            {
                // Check if provided credentials are valid
                if (!context.ValidateCredentials(user, password))
                {
                    Console.WriteLine("Invalid username or password.");
                    return;
                }
            }

            // Execute PowerShell Empire with elevated privileges
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe";
            startInfo.Arguments = $"{empirePath}\\empire --use-threadpool";
            startInfo.Verb = "runas";
            Process.Start(startInfo);
        }
    }
}
