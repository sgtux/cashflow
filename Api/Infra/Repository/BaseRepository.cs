using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Cashflow.Api.Shared;
using Dapper;
using Npgsql;

namespace Cashflow.Api.Infra.Repository
{
  public abstract class BaseRepository<T> where T : class
  {
    private NpgsqlConnection _connection;

    private string _connectionString;

    protected BaseRepository(AppConfig config) => _connectionString = config.ConnectionString;

    private IDbConnection Connection
    {
      get
      {
        if (_connection == null)
          _connection = new NpgsqlConnection(_connectionString);
        return _connection;
      }
    }

    protected Task Execute(string query, object parameters)
    {
      using (IDbConnection conn = Connection)
      {
        conn.Open();
        return conn.QueryFirstOrDefaultAsync<T>(query, parameters);
      }
    }

    protected Task<T> FirstOrDefault(string query, object parameters)
    {
      Connection.Open();
      return Connection.QuerySingleAsync<T>(query, parameters);
    }

    protected Task<IEnumerable<T>> Many(string query, object parameters)
    {
      using (IDbConnection conn = Connection)
      {
        conn.Open();
        return conn.QueryAsync<T>(query, parameters);
      }
    }
  }
}