using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Services;
using Cashflow.Api.Shared.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Cashflow.Api.Auth
{
    public class AuthenticationMiddleware(RequestDelegate _next, SecurityCache _securityCache)
    {
        private static string[] _staticFileExtensions = { ".ttf", ".gif", ".html", ".css", ".js", ".ico", ".woff2" };

        private const string AuthorizationHeader = "Authorization";
        
        private LogService _logService;

        public async Task Invoke(HttpContext context, IAppConfig appConfig, LogService logService)
        {
            _logService = logService;
            var path = context.Request.Path.Value;
            if (IsStaticFile(path))
            {
                await _next(context);
                return;
            }

            var endpoint = context.GetEndpoint();
            if (endpoint != null && endpoint.Metadata.GetMetadata<AuthorizeAttribute>() == null)
            {
                await _next(context);
                return;
            }

            if (context.Request.Headers.ContainsKey(AuthorizationHeader))
            {
                var token = context.Request.Headers[AuthorizationHeader].ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);

                if (await ValidateToken(token, appConfig.SecretJwtKey, context))
                {
                    await _next(context);
                    return;
                }
            }

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
        }

        private async Task<bool> ValidateToken(string token, string secretKey, HttpContext context)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
                var alg = jwtSecurityToken.Header.Alg;

                if (alg == SecurityAlgorithms.HmacSha256)
                {
                    var key = Encoding.ASCII.GetBytes(secretKey);

                    context.User = tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true
                    }, out _);

                    return true;
                }

                if (alg == SecurityAlgorithms.RsaSha256)
                {
                    var kid = jwtSecurityToken.Header.Kid;
                    var publicKey = await _securityCache.GetSecurityOauthPublicKey(kid);
                    if (publicKey == null)
                    {
                        _logService.Error($"PublicKey for token with Kid {kid} was not found.");
                        return false;
                    }

                    var rsa = RSA.Create();
                    rsa.ImportParameters(new RSAParameters()
                    {
                        Modulus = Base64UrlDecode(publicKey.N),
                        Exponent = Base64UrlDecode(publicKey.E)
                    });
                    context.User = tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new RsaSecurityKey(rsa),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true
                    }, out _);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                var logService = new LogService();
                logService.Error(ex.StackTrace);
                logService.Debug(ex.StackTrace);
                return false;
            }
        }

        private bool IsStaticFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            return _staticFileExtensions.Any(ext => path.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }

        private static byte[] Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            switch (s.Length % 4) { case 2: s += "=="; break; case 3: s += "="; break; }
            return Convert.FromBase64String(s);
        }
    }
}