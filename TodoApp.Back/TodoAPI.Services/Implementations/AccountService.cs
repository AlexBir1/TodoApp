using Microsoft.Extensions.Configuration;
using System.Security.Principal;
using TodoAPI.APIResponse.Implementations;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.DAL.Repositories.Interfaces;
using TodoAPI.Services.Interfaces;
using TodoAPI.Services.Token;
using TodoAPI.Shared.Models;

namespace TodoAPI.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly ICollectionRepository _collectionRepo;
        private readonly IConfiguration _config;

        public AccountService(IAccountRepository accountRepo, ICollectionRepository collectionRepo, IConfiguration config)
        {
            _accountRepo = accountRepo;
            _collectionRepo = collectionRepo;
            _config = config;
        }

        public async Task<IAPIResponse<AccountModel>> GetByIdAsync(string id)
        {
            var account = await _accountRepo.GetByIdAsync(id);
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
            var account = await _accountRepo.GetByIdAsync(model.AccountId);

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
            var result = await _accountRepo.GetAllAsync(x => x.Email == model.UserIdentifier || x.UserName == model.UserIdentifier || x.PhoneNumber == model.UserIdentifier);

            if (!result.Any())
                throw new Exception("Invalid login or password.");

            if (!await _accountRepo.CheckPasswordAsync(result.First(), model.Password))
                throw new Exception("Invalid login or password.");

            var token = TokenMaker.CreateToken(result.First(), new TokenDescriptorModel
            {
                Key = _config["JWT:Key"]!,
                ExpiresInDays = Convert.ToInt32(_config["JWT:ExpiresInDays"]!),
            });

            return new APIResponse<AuthorizationModel>(true, new AuthorizationModel
            {
                AccountId = result.First().Id,
                Token = token.Token,
                TokenExpirationDate = token.ValidTo,
                KeepAuthorized = model.KeepAuthorized
            });
        }

        public async Task<IAPIResponse<AuthorizationModel>> SignUpAsync(SignUpModel model)
        {
            var result = await _accountRepo.CreateAsync(new Account
            {
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Username
            });

            bool passwordCreated = await _accountRepo.SetPassword(result, model.PasswordConfirm);

            if (!passwordCreated)
            {
                await _accountRepo.DeleteAsync(result.Id);
                throw new Exception("Error while saving account password.");
            }

            await _collectionRepo.CreateAsync(new Collection
            {
                Title = "Unsorted",
                AccountId = result.Id,
            });

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
