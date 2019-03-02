using System;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;
using Cashflow.Api.Models;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Service
{
  /// Account service
  public class AccountService : BaseService
  {
    private IUserRepository _userRepository;

    /// Constructor
    public AccountService(IUserRepository repository) => _userRepository = repository;

    /// Get user by id
    public UserDataModel GetById(int userId)
    {
      var user = _userRepository.GetById(userId);
      return new UserDataModel(user);
    }


    /// Create a new account
    public UserDataModel Add(AccountModel model)
    {

      if (string.IsNullOrEmpty(model.Email) || !model.Email.Contains("@") || model.Email.Length < 5)
        ThrowValidationError("O email é inválido.");

      if (string.IsNullOrEmpty(model.Name))
        ThrowValidationError("O nome é obrigatório.");

      if (string.IsNullOrEmpty(model.Password) || model.Password.Length < 4)
        ThrowValidationError("Informe uma senha de pelo menos 4 dígitos.");

      User user = _userRepository.FindByNameEmail(model.Name, model.Email);

      if (user != null)
      {
        if (user.Name == model.Name)
          ThrowValidationError("O nome informado já está sendo usado.");
        ThrowValidationError("O email informado já está sendo usado.");
      }

      user = new User();
      user.Name = model.Name;
      user.Email = model.Email;
      user.Password = Utils.Sha1(model.Password);
      user.CreatedAt = DateTime.Now;

      _userRepository.Add(user);
      _userRepository.Save();

      user = _userRepository.FindByEmail(model.Email);

      return new UserDataModel(user);
    }
  }
}