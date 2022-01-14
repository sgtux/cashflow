using System;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Models;
using Cashflow.Api.Shared;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Services
{
    public class AccountService : BaseService
    {
        private IUserRepository _userRepository;

        public AccountService(IUserRepository repository)
        {
            _userRepository = repository;
        }

        public async Task<ResultModel> GetById(int userId)
        {
            var user = await _userRepository.GetById(userId);
            return new ResultDataModel<UserDataModel>(new UserDataModel(user));
        }

        public async Task<ResultDataModel<UserDataModel>> Add(User model)
        {
            var result = new ResultDataModel<UserDataModel>();
            var validationResults = new UserValidator(_userRepository).Validate(model);
            if (validationResults.IsValid)
            {
                model.Password = Utils.Sha1(model.Password);
                model.CreatedAt = CurrentDate;

                await _userRepository.Add(model);
                var user = await _userRepository.FindByNickName(model.NickName);
                user.Map(result.Data);
            }
            else
                result.AddNotification(validationResults.Errors);

            return result;
        }

        public async Task<ResultDataModel<UserDataModel>> Login(string nickName, string password)
        {
            var result = new ResultDataModel<UserDataModel>();

            if (string.IsNullOrEmpty(nickName) || string.IsNullOrEmpty(password))
            {
                result.AddNotification(ValidatorMessages.User.LoginFailed);
                return result;
            }

            var user = await _userRepository.FindByNickName(nickName);

            if (user == null || user.Password != Utils.Sha1(password))
            {
                result.AddNotification(ValidatorMessages.User.LoginFailed);
                return result;
            }

            result.Data = new UserDataModel(user);

            return result;
        }
    }
}