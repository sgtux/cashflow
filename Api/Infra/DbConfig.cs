using System.Data;

namespace Cashflow.Api.Infra
{
  public class DbConfig
  {
    public DbConfig(IDbConnection connection, string connectionString)
    {
      Connection = connection;
      ConnectionString = connectionString;
    }

    public IDbConnection Connection { get; private set; }

    public string ConnectionString { get; private set; }
  }
}