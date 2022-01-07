using System.Data;

namespace Cashflow.Api.Contracts
{
    public interface IDatabaseContext
    {
        IDbConnection Connection { get; }

        IDbTransaction Transaction { get; }

        IDbTransaction BeginTransaction();

        void Commit();

        void Rollback();
    }
}