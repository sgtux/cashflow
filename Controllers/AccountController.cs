using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using FinanceApi.Auth;
using FinanceApi.Infra;
using FinanceApi.Infra.Entity;
using FinanceApi.Models;
using FinanceApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.Controllers
{
  [Route("api/[controller]")]
  public class AccountController : BaseController
  {
    private AppDbContext _context;
    private AppConfiguration _config;

    public AccountController(AppDbContext context, AppConfiguration config)
    {
      _config = config;
      _context = context;
    }

    [Authorize]
    [HttpGet]
    public IActionResult Get()
    {
      var user = _context.User.FirstOrDefault(p => p.Id == UserId);
      return Ok(user);
    }

    [HttpPost]
    public IActionResult Post([FromBody]AccountModel model)
    {

      if (string.IsNullOrEmpty(model.Email) || !model.Email.Contains("@") || model.Email.Length < 5)
        ThrowValidationError("O Email é inválido.");

      if (string.IsNullOrEmpty(model.Name))
        ThrowValidationError("O Nome é obrigatório.");

      if (string.IsNullOrEmpty(model.Password) || model.Password.Length < 4)
        ThrowValidationError("Informe uma senha de pelo menos 4 dígitos.");

      User user = _context.User.FirstOrDefault(p => p.Email == model.Email || p.Name == model.Name);

      if (user != null)
      {
        if (user.Name == model.Name)
          ThrowValidationError("O Nome informado já está sendo usado.");
        ThrowValidationError("O Email informado já está sendo usado.");
      }

      user = new User();
      user.Name = model.Name;
      user.Email = model.Email;
      user.Password = Utils.Sha1(model.Password);
      user.CreatedAt = DateTime.Now;

      _context.User.Add(user);
      _context.SaveChanges();

      user = _context.User.First(p => p.Email == model.Email);

      var claims = new Dictionary<string, string>();
      claims.Add(ClaimTypes.Sid, user.Id.ToString());
      var token = new JwtTokenBuilder(_config.JwtKey, claims).Build();
      return Ok(new { token = token.Value });
    }
  }
}