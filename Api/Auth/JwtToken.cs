using System;
using System.IdentityModel.Tokens.Jwt;

namespace Cashflow.Api.Auth
{
    public sealed class JwtToken
    {
        private readonly JwtSecurityToken _token;

        public JwtToken(JwtSecurityToken token) => _token = token;

        public string Value => new JwtSecurityTokenHandler().WriteToken(_token);
    }
}