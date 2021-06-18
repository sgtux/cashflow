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

        public async Task<UserDataModel> GetById(int userId)
        {
            var user = await _userRepository.GetById(userId);
            return new UserDataModel(user);
        }

        public async Task<ResultDataModel<UserDataModel>> Add(User model)
        {
            var result = new ResultDataModel<UserDataModel>();
            var validationResults = new UserValidator(_userRepository).Validate(model);
            if (validationResults.IsValid)
            {
                model.Password = Utils.Sha1(model.Password);
                model.CreatedAt = DateTime.Now;

                await _userRepository.Add(model);
                var user = await _userRepository.FindByNickName(model.NickName);
                user.Map(result.Data);
            }
            else
                result.AddNotification(validationResults.Errors);

            return result;
        }

        public async Task<User> Login(string nickName, string password)
        {
            var user = await _userRepository.FindByNickName(nickName);
            return user?.Password == Utils.Sha1(password) ? user : null;
        }
    }
}