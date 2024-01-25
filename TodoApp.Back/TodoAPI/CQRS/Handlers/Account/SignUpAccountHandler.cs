using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Account;
using TodoAPI.Services.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.CQRS.Handlers.Account
{
    public class SignUpAccountHandler : IRequestHandler<SignUpAccountCommand, IAPIResponse<AuthorizationModel>>
    {
        private readonly IAccountService _accountService;

        public SignUpAccountHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IAPIResponse<AuthorizationModel>> Handle(SignUpAccountCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.SignUpAsync(request.model);
        }
    }
}
