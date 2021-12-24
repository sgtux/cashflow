using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using Cashflow.Migrations.DDL;

namespace Cashflow.Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            var downMigrations = args.Contains("down") || args.Contains("--down");
            string connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("Environment variable DATABASE_URL was not found.");

            var runner = new MigrationRunner(DatabaseType.Postgres, connectionString, true, typeof(AddUserTable).Assembly);
            runner.Run(downMigrations);
        }
    }
}