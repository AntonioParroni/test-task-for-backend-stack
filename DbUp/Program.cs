using System;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace DbUp
{
    class Program
    {
        static int Main(string[] args)
        {
            Thread.Sleep(30000);
            var connectionString =
                args.FirstOrDefault()
                ?? "Server=db;Database=Contoso_Authentication_Logs;Trusted_Connection=True;User Id=sa;Password=Password123;"; // don't ever ever add here "Trusted_Connection=True;"
                // ?? "Server=localhost;Database=Contoso_Authentication_Logs;User Id=sa;Password=Password123;";

            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();
            
            var result = upgrader.PerformUpgrade();
            
            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                Console.ReadLine();
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}