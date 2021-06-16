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
            string defaultConnectionString = "Host=172.30.30.10;Port=5432;Pooling=true;User Id=postgres;Password=Postgres123";
            string connectionString = defaultConnectionString ?? Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(AddUserTable).Assembly).For.Migrations()
                    .ScanIn(typeof(AddCreditCardTable).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider, bool downMigrations)
        {
            var migrationsVersions = new List<int>() { 5, 4, 3, 2, 1, 0 };
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            if (downMigrations)
                foreach (var version in migrationsVersions)
                    runner.MigrateDown(version);
            else
                runner.MigrateUp();
        }
    }
}