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

        private readonly IAppConfig _config;

        public AccountService(IUserRepository repository, AppCache appCache, IAppConfig config)
        {
            _userRepository = repository;
            _appCache = appCache;
            _config = config;
        }

        public async Task<ResultModel> GetById(int userId)
        {
            var user = await _userRepository.GetById(userId);

            if (!user.Email.Contains("@"))
                user.Email = CryptographyUtils.AesDecrypt(user.Email, _config.DataEncryptionKey);
                
            return new ResultDataModel<UserDataModel>(new UserDataModel(user));
        }

        public async Task<ResultDataModel<UserDataModel>> Add(User model)
        {
            var result = new ResultDataModel<UserDataModel>();
            var validationResults = new UserValidator(_userRepository).Validate(model);
            if (validationResults.IsValid)
            {
                model.Password = CryptographyUtils.PasswordHash(model.Password);
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
            var encryptedEmail = CryptographyUtils.AesEncrypt(googleUserModel.email, _config.DataEncryptionKey);
            var user = await _userRepository.FindByEmail(encryptedEmail);

            if (user == null)
            {
                await _userRepository.Add(new User()
                {
                    CreatedAt = CurrentDate,
                    Email = encryptedEmail
                });
            }

            user = await _userRepository.FindByEmail(encryptedEmail);
            user.Email = googleUserModel.email;

            return new ResultDataModel<UserDataModel>(new UserDataModel(user));
        }

        public async Task<ResultModel> UpdateSpendingCeiling(int userId, decimal spendingCeiling)
        {
            var result = new ResultModel();
            if (spendingCeiling < 0 || spendingCeiling > 99999)
                result.AddNotification("Valor inválido.");
            else
            {
                await _userRepository.UpdateSpendingCeiling(userId, spendingCeiling);
                _appCache.Clear(userId);
            }
            return result;
        }
    }
}