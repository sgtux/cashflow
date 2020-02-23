using System;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;
using Cashflow.Api.Models;
using Cashflow.Api.Shared;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Service
{
  public class AccountService : BaseService
  {
    private IUserRepository _userRepository;

    public AccountService(IUserRepository repository) => _userRepository = repository;

    public UserDataModel GetById(int userId)
    {
      var user = _userRepository.GetById(userId);
      return new UserDataModel(user.Result);
    }

    public async Task<UserDataModel> Add(User model)
    {
      var result = new UserDataModel();
      var validationResults = new UserValidator().Validate(model);
      if(!validationResults.IsValid)
      {
        result.AddNotification(validationResults.Errors);
        return result;
      }

      User user = await _userRepository.FindByEmail(model.Email);
      if (user != null)
      {
        result.AddNotification("The email is already being used.");
        return result;
      }

      model.Password = Utils.Sha1(model.Password);
      model.CreatedAt = DateTime.Now;

      await _userRepository.Add(model);
      user = await _userRepository.FindByEmail(model.Email);
      user.Map(result);

      return result;
    }

    public async Task<User> Login(string email, string password)
    {
      password = Utils.Sha1(password);
      var user = await _userRepository.FindByEmail(email);
      return user?.Password == password ? user : null;
    }
  }
}