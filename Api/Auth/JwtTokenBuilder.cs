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
    private int expiryInMinutes = 60 * 24 * 7;

    private SecurityKey key;
    
    private Dictionary<string, string> claims;

    public JwtTokenBuilder(string key, Dictionary<string, string> claims)
    {
      this.key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
      this.claims = claims;
    }

    public JwtToken Build()
    {
      var token = new JwtSecurityToken(
        claims: claims.Select(item => new Claim(item.Key, item.Value)).ToList(),
          expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
          signingCredentials: new SigningCredentials(
              key,
              SecurityAlgorithms.HmacSha256
          )
      );
      return new JwtToken(token);
    }
  }
}