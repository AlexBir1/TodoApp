using Microsoft.Extensions.Configuration;
using System.Security.Principal;
using TodoAPI.APIResponse.Implementations;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.DAL.UOW.Interfaces;
using TodoAPI.Services.Interfaces;
using TodoAPI.Services.Token;
using TodoAPI.Shared.Models;

namespace TodoAPI.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IDBRepository _dBRepo;
        private readonly IConfiguration _config;

        public AccountService(IDBRepository dBRepo, IConfiguration config)
        {
            _dBRepo = dBRepo;
            _config = config;
        }

        public async Task<IAPIResponse<AccountModel>> GetByIdAsync(string id)
        {
            var account = await _dBRepo.AccountRepo.GetByIdAsync(id);
            return new APIResponse<AccountModel>(true, new AccountModel
            {
                Id = account.Id,
                Username = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
            });
        }

        public async Task<IAPIResponse<AuthorizationModel>> RefreshTokenAsync(AuthorizationModel model)
        {
            var account = await _dBRepo.AccountRepo.GetByIdAsync(model.AccountId);

            var token = TokenMaker.CreateToken(account, new TokenDescriptorModel
            {
                Key = _config["JWT:Key"]!,
                ExpiresInDays = Convert.ToInt32(_config["JWT:ExpiresInDays"]!),
            });

            return new APIResponse<AuthorizationModel>(true, new AuthorizationModel
            {
                AccountId = account.Id,
                Token = token.Token,
                TokenExpirationDate = token.ValidTo,
                KeepAuthorized = model.KeepAuthorized
            });
        }

        public async Task<IAPIResponse<AuthorizationModel>> SignInAsync(SignInModel model)
        {
            var account = await _dBRepo.AccountRepo.GetAllAsync(x => x.Email == model.UserIdentifier || x.UserName == model.UserIdentifier || x.PhoneNumber == model.UserIdentifier);

            if (account.Single() == null)
                throw new Exception("Invalid login or password.");

            if(!await _dBRepo.AccountRepo.CheckPasswordAsync(account.Single(), model.Password))
                throw new Exception("Invalid login or password.");

            var token = TokenMaker.CreateToken(account.Single(), new TokenDescriptorModel
            {
                Key = _config["JWT:Key"]!,
                ExpiresInDays = Convert.ToInt32(_config["JWT:ExpiresInDays"]!),
            });

            return new APIResponse<AuthorizationModel>(true, new AuthorizationModel
            {
                AccountId = account.Single().Id,
                Token = token.Token,
                TokenExpirationDate = token.ValidTo,
                KeepAuthorized = model.KeepAuthorized
            });
        }

        public async Task<IAPIResponse<AuthorizationModel>> SignUpAsync(SignUpModel model)
        {
            var result = await _dBRepo.AccountRepo.CreateAsync(new Account
            {
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Username
            });

            _dBRepo.AccountRepo.SetPassword(result, model.PasswordConfirm);

            var token = TokenMaker.CreateToken(result, new TokenDescriptorModel
            {
                Key = _config["JWT:Key"]!,
                ExpiresInDays = Convert.ToInt32(_config["JWT:ExpiresInDays"]!),
            });

            return new APIResponse<AuthorizationModel>(true, new AuthorizationModel
            {
                AccountId = result.Id,
                Token = token.Token,
                TokenExpirationDate = token.ValidTo,
                KeepAuthorized = model.KeepAuthorized
            });
        }
    }
}
