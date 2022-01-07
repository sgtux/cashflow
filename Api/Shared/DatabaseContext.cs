using System.Data;
using Cashflow.Api.Contracts;
using Npgsql;

namespace Cashflow.Api.Shared
{
    public class DatabaseContext : IDatabaseContext
    {
        private IDbConnection _conn;

        private string _connectionString;

        public IDbConnection Connection => _conn;

        public IDbTransaction Transaction { get; private set; }

        public DatabaseContext(IAppConfig config)
        {
            _connectionString = config.DatabaseConnectionString;
            _conn = new NpgsqlConnection(_connectionString);
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