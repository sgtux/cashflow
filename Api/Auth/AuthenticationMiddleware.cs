using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Cashflow.Api.Auth
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        private string[] _staticFileExtensions = { ".ttf", ".gif", ".html", ".css", ".js", ".ico" };

        private const string AuthorizationHeader = "Authorization";

        public AuthenticationMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, IAppConfig appConfig)
        {
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

                if (ValidateToken(token, appConfig.SecretJwtKey, context))
                {
                    await _next(context);
                    return;
                }
            }

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
        }

        private bool ValidateToken(string token, string secretKey, HttpContext context)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
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
            catch (Exception ex)
            {
                var logService = new LogService();
                logService.Error(ex.StackTrace);
                logService.Debug(ex.StackTrace);
                logService.Debug(token);
                return false;
            }
        }

        private bool IsStaticFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            return _staticFileExtensions.Any(ext => path.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }
    }
}