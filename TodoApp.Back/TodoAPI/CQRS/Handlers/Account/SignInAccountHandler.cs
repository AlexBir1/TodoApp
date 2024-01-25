using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Account;
using TodoAPI.Services.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.CQRS.Handlers.Account
{
    public class SignInAccountHandler : IRequestHandler<SignInAccountCommand, IAPIResponse<AuthorizationModel>>
    {
        private readonly IAccountService _accountService;

        public SignInAccountHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IAPIResponse<AuthorizationModel>> Handle(SignInAccountCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.SignInAsync(request.model);
        }
    }
}
