using System;
using System.Linq;
using System.Reflection;

namespace DbUp
{
    class Program
    {
        static int Main(string[] args)
        {
            
            var connectionString =
                args.FirstOrDefault()
                ?? "Server=localhost;Database=testDB;User Id=sa;Password=Password123;";
            
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