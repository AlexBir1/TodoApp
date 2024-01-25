using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;

namespace TodoAPI.CQRS.Queries.Goals
{
    public record GetGoalListByCollectionIdPagedQuery(string collectionId, string searchQuery, int selectedPage, int itemsPerPage) : IRequest<IAPIResponse<IEnumerable<Goal>>>;
}
