using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace Cashflow.Api.Infra.Repository
{
  public abstract class BaseRepository<T> where T : class
  {
    private NpgsqlConnection _connection;

    private DbConfig _dbConfig;

    public BaseRepository(DbConfig dbConfig) => _dbConfig = dbConfig;

    public IDbConnection Connection
    {
      get
      {
        if (_connection == null)
          _connection = new NpgsqlConnection(_dbConfig.ConnectionString);
        return _connection;
      }
    }

    protected Task<T> FirstOrDefault(string query, object parameters) 
    {
      using (IDbConnection conn = Connection)
      {
        conn.Open();
        return conn.QueryFirstOrDefaultAsync<T>(query, parameters);
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

    protected Task<IEnumerable<T>> QueryMany(string query, object parameters)
    {
      using (IDbConnection conn = Connection)
      {
        conn.Open();
        return conn.QueryAsync<T>(query, parameters);
      }
    }
  }
}