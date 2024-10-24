using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Models;
using Cashflow.Api.Shared.Cache;
using Cashflow.Api.Utils;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Services
{
    public class AccountService : BaseService
    {
        private readonly IUserRepository _userRepository;

        private readonly AppCache _appCache;

        private readonly ISystemParameterRepository _systemParameterRepository;

        public AccountService(IUserRepository repository, AppCache appCache, ISystemParameterRepository systemParameterRepository)
        {
            _userRepository = repository;
            _appCache = appCache;
            _systemParameterRepository = systemParameterRepository;
        }

        public async Task<ResultModel> GetById(int userId)
        {
            var user = await _userRepository.GetById(userId);
            return new ResultDataModel<UserDataModel>(new UserDataModel(user));
        }

        public async Task<ResultDataModel<UserDataModel>> Add(User model)
        {
            var result = new ResultDataModel<UserDataModel>();

            if (await _userRepository.Count() >= await _systemParameterRepository.MaximumSystemUsers())
            {
                result.AddNotification(ValidatorMessages.User.MaximumSystemUsers);
                return result;
            }

            var validationResults = new UserValidator(_userRepository).Validate(model);
            if (!validationResults.IsValid)
            {
                result.AddNotification(validationResults.Errors);
                return result;
            }

            model.Password = CryptographyUtils.PasswordHash(model.Password);
            model.CreatedAt = CurrentDate;

            await _userRepository.Add(model);
            var user = await _userRepository.FindByEmail(model.Email);
            user.Map(result.Data);

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

            if (user == null || !CryptographyUtils.PasswordHashVarify(password, user.Password))
            {
                result.AddNotification(ValidatorMessages.User.LoginFailed);
                return result;
            }

            result.Data = new UserDataModel(user);

            return result;
        }

        public async Task<ResultDataModel<UserDataModel>> Login(GoogleUserModel googleUserModel)
        {
            var user = await _userRepository.FindByEmail(googleUserModel.email);

            var result = new ResultDataModel<UserDataModel>();

            if (user == null)
            {
                if (await _userRepository.Count() >= await _systemParameterRepository.MaximumSystemUsers())
                {
                    result.AddNotification(ValidatorMessages.User.MaximumSystemUsers);
                    return result;
                }

                await _userRepository.Add(new User()
                {
                    CreatedAt = CurrentDate,
                    Email = googleUserModel.email
                });
            }

            user = await _userRepository.FindByEmail(googleUserModel.email);
            result.Data = new UserDataModel(user);

            return result;
        }

        public async Task<ResultDataModel<UserDataModel>> Update(User user)
        {
            var result = new ResultDataModel<UserDataModel>(new UserDataModel(user));
            const int minValue = 0;
            const int maxValue = 99999;
            if (user.ExpenseLimit < minValue || user.ExpenseLimit > maxValue)
                result.AddNotification(ValidatorMessages.BetweenValue("ExpenseLimit", minValue, maxValue));
            else if (user.FuelExpenseLimit < minValue || user.FuelExpenseLimit > maxValue)
                result.AddNotification(ValidatorMessages.BetweenValue("FuelExpenseLimit", minValue, maxValue));
            else
            {
                await _userRepository.Update(user);
                _appCache.Clear(user.Id);
            }
            return result;
        }
    }
}