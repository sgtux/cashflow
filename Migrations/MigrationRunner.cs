using System;
using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Cashflow.Migrations
{
    public enum DatabaseType
    {
        Postgres,

        Sqlite
    }

    public class MigrationRunner
    {
        private static DatabaseType _type;

        private string _connectionString;

        private Assembly[] _assembliesToScan;

        private IServiceProvider _serviceProvider;

        private bool _useConsoleLog;

        public MigrationRunner(DatabaseType type, string connectionString, bool useConsoleLog, params Assembly[] assembliesToScan)
        {
            _type = type;
            _connectionString = connectionString;
            _assembliesToScan = assembliesToScan;
            _useConsoleLog = useConsoleLog;
            Configure();
        }

        public void Run(bool downMigrations = false)
        {
            var runner = _serviceProvider.GetRequiredService<IMigrationRunner>();
            if (downMigrations)
            {
                var count = runner.MigrationLoader.LoadMigrations().Count;
                for (int i = count; i >= 0; i--)
                    runner.MigrateDown(i);
            }
            else
                runner.MigrateUp();
        }

        private void Configure()
        {
            _serviceProvider = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb =>
                {
                    if (_type == DatabaseType.Postgres)
                    {
                        rb.AddPostgres()
                            .WithGlobalConnectionString(_connectionString)
                            .ScanIn(_assembliesToScan).For.Migrations();
                    }
                    else
                    {
                        rb.AddSQLite()
                            .WithGlobalConnectionString(_connectionString)
                            .ScanIn(_assembliesToScan).For.Migrations();
                    }
                })
                .AddLogging(lb =>
                {
                    if (_useConsoleLog)
                        lb.AddFluentMigratorConsole();
                })
                .BuildServiceProvider(false);
        }
    }
}