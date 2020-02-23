using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Cashflow.Api.Extensions;
using Cashflow.Api.Shared;
using Dapper;
using Npgsql;

namespace Cashflow.Api.Infra.Repository
{
  public abstract class BaseRepository<T> where T : class
  {
    private string _connectionString;

    private IDbConnection Connection => new NpgsqlConnection(_connectionString);

    protected BaseRepository(AppConfig config) => _connectionString = config.ConnectionString;

    protected async Task Execute(string query, object parameters)
    {
      query = await query.GetResource();
      Log(query);
      using (var conn = Connection)
        await conn.ExecuteAsync(query, parameters);
    }

    protected async Task<T> FirstOrDefault(string query, object parameters)
    {
      query = await query.GetResource();
      Log(query);
      using (var conn = Connection)
        return await conn.QueryFirstOrDefaultAsync<T>(query, parameters);
    }

    protected async Task<IEnumerable<T>> Many(string query, object parameters = null)
    {
      query = await query.GetResource();
      Log(query);
      using (var conn = Connection)
        return await conn.QueryAsync<T>(query, parameters);
    }

    public async Task<bool> Exists(long id)
    {
      var query = $"SELECT COUNT(1) FROM \"{typeof(T).Name}\" WHERE \"Id\" = @Id";
      Log(query);
      using (var conn = Connection)
        return await conn.ExecuteScalarAsync<long>(query, new { Id = id }) > 0;
    }

    private void Log(string query)
    {
      System.Diagnostics.Debug.WriteLine("");
      System.Diagnostics.Debug.WriteLine("");
      System.Diagnostics.Debug.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} - Query:{query}");
      System.Diagnostics.Debug.WriteLine("");
      System.Diagnostics.Debug.WriteLine("");
    }
  }
}