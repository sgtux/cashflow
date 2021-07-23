using System;
using System.IdentityModel.Tokens.Jwt;

namespace Cashflow.Api.Auth
{
    public sealed class JwtToken
    {
        private JwtSecurityToken token;

        public JwtToken(JwtSecurityToken token) => this.token = token;

        public string Value => new JwtSecurityTokenHandler().WriteToken(token);
    }
}