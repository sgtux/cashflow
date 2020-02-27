using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Api.Infra.Resources;
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

    protected async Task Execute(ResourceBuilder resource, object parameters = null)
    {
      var query = await resource.Build();
      Log(query);
      using (var conn = Connection)
        await conn.ExecuteAsync(query, parameters);
    }

    public async Task<U> ExecuteScalar<U>(ResourceBuilder resource, object parameters)
    {
      var query = await resource.Build();
      Log(query);
      using (var conn = Connection)
        return await conn.ExecuteScalarAsync<U>(query, parameters);
    }

    protected async Task<T> FirstOrDefault(ResourceBuilder resource, object parameters)
    {
      var query = await resource.Build();
      Log(query);
      using (var conn = Connection)
        return await conn.QuerySingleOrDefaultAsync<T>(query, parameters);
    }

    protected async Task<IEnumerable<T>> Query(ResourceBuilder resource, object parameters = null)
    {
      var query = await resource.Build();
      Log(query);
      using (var conn = Connection)
        return await conn.QueryAsync<T>(query, parameters);
    }

    protected async Task<IEnumerable<T>> Query<U>(ResourceBuilder resource, Func<T, U, T> map, object parameters = null)
    {
      var query = await resource.Build();
      Log(query);
      using (var conn = Connection)
        return await conn.QueryAsync<T, U, T>(query, map, parameters);
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