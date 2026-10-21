using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace Cashflow.Api.Shared.Cache
{
    public class SecurityCache(IAppConfig _appConfig, IMemoryCache _memoryCache)
    {
        private const string SECURITY_OAUTH_PUBLIC_KEY = "@SECURITY_OAUTH_PUBLIC_KEY";

        public async Task<SecurityOauthKeyPartsModel> GetSecurityOauthPublicKey(string kid)
        {
            var keys = await _memoryCache.GetOrCreateAsync(SECURITY_OAUTH_PUBLIC_KEY, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
                using HttpClient client = new();
                HttpResponseMessage response = await client.GetAsync($"{_appConfig.AuthApiUrl}/.well-known/jwks.json");
                string responseBody = await response.Content.ReadAsStringAsync();
                var model = JsonSerializer.Deserialize<SecurityOAuthKeyModel>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return model.Keys;
            });

            return keys.FirstOrDefault(p => p.Alg == SecurityAlgorithms.RsaSha256 && p.Kid == kid);
        }
    }
}