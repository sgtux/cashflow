using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Api.Infra.Resources;
using Cashflow.Api.Service;
using Cashflow.Api.Shared;
using Dapper;
using static System.Console;

namespace Cashflow.Api.Infra.Repository
{
    public abstract class BaseRepository<T> where T : class
    {
        private readonly IDbConnection _conn;

        private readonly DatabaseContext _context;

        private readonly LogService _logService;

        public IDbTransaction Transaction { get; private set; }

        protected BaseRepository(DatabaseContext context, LogService logService)
        {
            _context = context;
            _conn = context.Connection;
            _logService = logService;
        }

        protected async Task Execute(ResourceBuilder resource, object parameters = null, IDbTransaction transaction = null)
        {
            var query = await resource.Build();
            Log(query);
            await _conn.ExecuteAsync(query, parameters, transaction ?? Transaction);
        }

        protected async Task<U> ExecuteScalar<U>(ResourceBuilder resource, object parameters, IDbTransaction transaction = null)
        {
            var query = await resource.Build();
            Log(query);
            return await _conn.ExecuteScalarAsync<U>(query, parameters, transaction ?? Transaction);
        }

        protected async Task<T> FirstOrDefault(ResourceBuilder resource, object parameters, IDbTransaction transaction = null)
        {
            var query = await resource.Build();
            Log(query);
            return await _conn.QuerySingleOrDefaultAsync<T>(query, parameters, transaction ?? Transaction);
        }

        protected async Task<IEnumerable<T>> Query(ResourceBuilder resource, object parameters = null, IDbTransaction transaction = null)
        {
            var query = await resource.Build();
            Log(query);
            return await _conn.QueryAsync<T>(query, parameters, transaction ?? Transaction);
        }

        protected async Task<IEnumerable<U>> Query<U>(ResourceBuilder resource, object parameters = null, IDbTransaction transaction = null)
        {
            var query = await resource.Build();
            Log(query);
            return await _conn.QueryAsync<U>(query, parameters, transaction ?? Transaction);
        }

        protected async Task<IEnumerable<T>> Query<U>(ResourceBuilder resource, Func<T, U, T> map, object parameters = null, IDbTransaction transaction = null)
        {
            var query = await resource.Build();
            Log(query);
            return await _conn.QueryAsync<T, U, T>(query, map, parameters, transaction ?? Transaction);
        }

        public IDbTransaction BeginTransaction()
        {
            Transaction = _context.BeginTransaction();
            return Transaction;
        }

        public void Commit() => _context.Commit();

        public void Rollback() => _context.Rollback();

        public async Task<bool> Exists(long id)
        {
            var query = $"SELECT COUNT(1) FROM \"{typeof(T).Name}\" WHERE \"Id\" = @Id";
            Log(query);
            return await _conn.ExecuteScalarAsync<long>(query, new { Id = id }) > 0;
        }

        public Task<long> NextId()
        {
            var query = $"SELECT MAX(\"Id\") FROM \"{typeof(T).Name}\"";
            Log(query);
            return _conn.ExecuteScalarAsync<long>(query);
        }

        private void Log(string query) => _logService.Info($"\n\nContextId: {_context.Id} - ThreadId: {Thread.CurrentThread.ManagedThreadId} - Query:{query}\n\n");
    }
}