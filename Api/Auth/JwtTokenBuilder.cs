using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Cashflow.Api.Auth
{
    public class JwtTokenBuilder
    {
        private readonly int _expiryInMinutes;

        private readonly SecurityKey _key;

        private readonly Dictionary<string, string> _claims;

        public JwtTokenBuilder(string key, int expiryInMinutes, Dictionary<string, string> claims)
        {
            _key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
            _claims = claims;
            _expiryInMinutes = expiryInMinutes;
        }

        public JwtToken Build()
        {
            var token = new JwtSecurityToken(
              claims: _claims.Select(item => new Claim(item.Key, item.Value)).ToList(),
                expires: DateTime.UtcNow.AddMinutes(_expiryInMinutes),
                signingCredentials: new SigningCredentials(
                    _key,
                    SecurityAlgorithms.HmacSha256
                )
            );
            return new JwtToken(token);
        }
    }
}