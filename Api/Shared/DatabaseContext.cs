using System.Data;
using Npgsql;

namespace Cashflow.Api.Shared
{
    public class DatabaseContext
    {
        private static int _id;
        public readonly IDbConnection Connection;
        public readonly int Id;
        public IDbTransaction Transaction { get; private set; }
        public DatabaseContext(AppConfig config)
        {
            Id = ++_id;
            string conn = config.DatabaseConnectionString;
            Connection = new NpgsqlConnection(conn);
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
        }

        public void Rollback()
        {
            Transaction?.Rollback();
        }
    }
}