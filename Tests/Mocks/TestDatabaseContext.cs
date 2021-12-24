using System.Data;
using Cashflow.Api.Contracts;
using Cashflow.Tests.Config;
using Microsoft.Data.Sqlite;

namespace Cashflow.Tests.Mocks
{
    public class TestDatabaseContext : IDatabaseContext
    {
        private IDbConnection _conn;

        private string _connectionString;

        public IDbConnection Connection => _conn;

        public IDbTransaction Transaction { get; private set; }

        public TestDatabaseContext()
        {
            _connectionString = new TestAppConfig().DatabaseConnectionString;
            _conn = new SqliteConnection(_connectionString);
        }

        public IDbTransaction BeginTransaction()
        {
            Connection.Open();
            Transaction = Connection.BeginTransaction();
            return Transaction;
        }

        public void Commit()
        {
            Transaction?.Commit();
            if (Connection?.State == ConnectionState.Open)
                Connection.Close();
        }

        public void Rollback()
        {
            Transaction?.Rollback();
            if (Connection?.State == ConnectionState.Open)
                Connection.Close();
        }
    }
}