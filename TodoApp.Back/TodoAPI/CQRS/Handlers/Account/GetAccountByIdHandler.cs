using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Account;
using TodoAPI.CQRS.Queries.Account;
using TodoAPI.Services.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.CQRS.Handlers.Account
{
    public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, IAPIResponse<AccountModel>>
    {
        private readonly IAccountService _accountService;

        public GetAccountByIdHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IAPIResponse<AccountModel>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            return await _accountService.GetByIdAsync(request.accountId);
        }
    }
}
