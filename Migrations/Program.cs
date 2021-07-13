using System;
using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using Cashflow.Migrations.DDL;
using System.Linq;
using System.Collections.Generic;

namespace Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            var downMigrations = args.Contains("down") || args.Contains("--down");
            var serviceProvider = CreateServices();

            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider, downMigrations);
            }
        }

        private static IServiceProvider CreateServices()
        {
            string connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("Environment DATABASE_URL not found.");
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(AddUserTable).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider, bool downMigrations)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            if (downMigrations)
            {
                var count = runner.MigrationLoader.LoadMigrations().Count;
                for (int i = count; i >= 0; i--)
                    runner.MigrateDown(i);
            }
            else
                runner.MigrateUp();
        }
    }
}