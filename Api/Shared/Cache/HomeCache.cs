using System.Collections.Generic;
using Cashflow.Api.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Cashflow.Api.Shared.Cache
{
    public class HomeCache
    {
        private const string KEY_PREFIX = "@HOME_";

        private IMemoryCache _memoryCache;

        public HomeCache(IMemoryCache memoryCache) => _memoryCache = memoryCache;

        public List<HomeChartModel> Get(int userId) => _memoryCache.Get<List<HomeChartModel>>(BuildKey(userId));

        public void Update(int userId, List<HomeChartModel> list) => _memoryCache.Set(BuildKey(userId), list);

        public void Clear(int userId) => _memoryCache.Remove(BuildKey(userId));

        private string BuildKey(int userId) => $"{KEY_PREFIX}{userId}";
    }
}