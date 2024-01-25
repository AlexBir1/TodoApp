using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.CQRS.Queries.Account
{
    public record GetAccountByIdQuery(string accountId) : IRequest<IAPIResponse<AccountModel>>;
}
