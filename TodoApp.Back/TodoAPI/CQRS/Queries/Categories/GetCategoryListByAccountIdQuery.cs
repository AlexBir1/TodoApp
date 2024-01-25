using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;

namespace TodoAPI.CQRS.Queries.Categories
{
    public record GetCategoryListByAccountIdQuery(string accountId) : IRequest<IAPIResponse<IEnumerable<Category>>>;
}
