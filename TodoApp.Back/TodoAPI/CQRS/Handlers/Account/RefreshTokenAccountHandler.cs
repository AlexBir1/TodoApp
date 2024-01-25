using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Account;
using TodoAPI.Services.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.CQRS.Handlers.Account
{
    public class RefreshTokenAccountHandler : IRequestHandler<RefreshTokenAccountCommand, IAPIResponse<AuthorizationModel>>
    {
        private readonly IAccountService _accountService;

        public RefreshTokenAccountHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IAPIResponse<AuthorizationModel>> Handle(RefreshTokenAccountCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.RefreshTokenAsync(request.model);
        }
    }
}
