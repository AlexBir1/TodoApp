using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;

namespace TodoAPI.CQRS.Queries.Collections
{
    public record GetCollectionListByAccountIdQuery(string accountId) : IRequest<IAPIResponse<IEnumerable<Collection>>>;
}
