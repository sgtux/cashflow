using Cashflow.Api.Models.Home;
using Microsoft.Extensions.Caching.Memory;

namespace Cashflow.Api.Shared.Cache
{
    public class HomeCache
    {
        private const string KEY_PREFIX = "@HOME_";

        private IMemoryCache _memoryCache;

        public HomeCache(IMemoryCache memoryCache) => _memoryCache = memoryCache;

        public HomeModel Get(int userId) => _memoryCache.Get<HomeModel>(BuildKey(userId));

        public void Update(int userId, HomeModel homeModel) => _memoryCache.Set(BuildKey(userId), homeModel);

        public void Clear(int userId) => _memoryCache.Remove(BuildKey(userId));

        private string BuildKey(int userId) => $"{KEY_PREFIX}{userId}";
    }
}