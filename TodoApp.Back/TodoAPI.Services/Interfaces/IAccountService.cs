using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IAPIResponse<AuthorizationModel>> SignUpAsync(SignUpModel model);
        Task<IAPIResponse<AuthorizationModel>> SignInAsync(SignInModel model);
        Task<IAPIResponse<AccountModel>> GetByIdAsync(string id);
        Task<IAPIResponse<AuthorizationModel>> RefreshTokenAsync(AuthorizationModel model);
    }
}
