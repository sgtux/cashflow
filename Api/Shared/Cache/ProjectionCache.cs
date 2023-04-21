using System.Collections.Generic;
using Cashflow.Api.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Cashflow.Api.Shared.Cache
{
    public class ProjectionCache
    {
        private const string KEY_PREFIX = "@PROJECTION_";

        private IMemoryCache _memoryCache;

        public ProjectionCache(IMemoryCache memoryCache) => _memoryCache = memoryCache;

        public List<PaymentMonthProjectionModel> Get(int userId) => _memoryCache.Get<List<PaymentMonthProjectionModel>>(BuildKey(userId));

        public void Update(int userId, List<PaymentMonthProjectionModel> list) => _memoryCache.Set(BuildKey(userId), list);

        public void Clear(int userId) => _memoryCache.Remove(BuildKey(userId));

        private string BuildKey(int userId) => $"{KEY_PREFIX}{userId}";
    }
}