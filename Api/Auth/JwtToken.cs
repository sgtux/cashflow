using System;
using System.IdentityModel.Tokens.Jwt;

namespace Cashflow.Api.Auth
{
  /// Jwt Token
  public sealed class JwtToken
  {
    private JwtSecurityToken token;

    internal JwtToken(JwtSecurityToken token)
    {
      this.token = token;
    }

    /// Date expires token
    public DateTime ValidTo => token.ValidTo;

    /// Token
    public string Value => new JwtSecurityTokenHandler().WriteToken(token);
  }
}