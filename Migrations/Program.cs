using System;
using System.Linq;
using Cashflow.Migrations.DDL;

namespace Cashflow.Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            var downMigrations = args.Contains("down") || args.Contains("--down");
            string connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("Environment variable DATABASE_CONNECTION_STRING was not found.");

            var runner = new MigrationRunner(DatabaseType.Postgres, connectionString, true, typeof(AddUserTable).Assembly);
            runner.Run(downMigrations);
        }
    }
}