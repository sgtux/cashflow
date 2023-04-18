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
        private readonly IUserRepository _userRepository;

        private readonly ProjectionCache _projectionCache;

        public AccountService(IUserRepository repository, ProjectionCache projectionCache)
        {
            _userRepository = repository;
            _projectionCache = projectionCache;
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
                var user = await _userRepository.FindByEmail(model.Email);
                user.Map(result.Data);
            }
            else
                result.AddNotification(validationResults.Errors);

            return result;
        }

        public async Task<ResultDataModel<UserDataModel>> Login(string email, string password)
        {
            var result = new ResultDataModel<UserDataModel>();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                result.AddNotification(ValidatorMessages.User.LoginFailed);
                return result;
            }

            var user = await _userRepository.FindByEmail(email);

            if (user == null || user.Password != Utils.Sha1(password))
            {
                result.AddNotification(ValidatorMessages.User.LoginFailed);
                return result;
            }

            result.Data = new UserDataModel(user);

            return result;
        }

        public async Task<ResultModel> UpdateSpendingCeiling(int userId, decimal spendingCeiling)
        {
            var result = new ResultModel();
            if (spendingCeiling < 0 || spendingCeiling > 99999)
                result.AddNotification("Valor inválido.");
            else
            {
                await _userRepository.UpdateSpendingCeiling(userId, spendingCeiling);
                _projectionCache.Clear(userId);
            }
            return result;
        }
    }
}