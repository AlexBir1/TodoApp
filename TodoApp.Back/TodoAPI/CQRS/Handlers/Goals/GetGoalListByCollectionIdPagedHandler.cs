using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Queries.Goals;
using TodoAPI.DAL.Entities;
using TodoAPI.Services.Interfaces;

namespace TodoAPI.CQRS.Handlers.Goals
{
    public class GetGoalListByCollectionIdPagedHandler : IRequestHandler<GetGoalListByCollectionIdPagedQuery, IAPIResponse<IEnumerable<Goal>>>
    {
        private readonly IGoalService _goalService;

        public GetGoalListByCollectionIdPagedHandler(IGoalService goalService)
        {
            _goalService = goalService;
        }

        public async Task<IAPIResponse<IEnumerable<Goal>>> Handle(GetGoalListByCollectionIdPagedQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.searchQuery))
                return await _goalService.GetAllPagedAsync(x => x.Title.Contains(request.searchQuery) || x.Description.Contains(request.searchQuery) || x.GoalCategories.Any(x => x.Category.ColorTitle.Contains(request.searchQuery)), request.itemsPerPage, request.selectedPage);

            if (string.IsNullOrEmpty(request.collectionId))
                return await _goalService.GetAllPagedAsync(null, request.itemsPerPage, request.selectedPage);
            else
                return await _goalService.GetAllPagedAsync(x => x.CollectionId == Guid.Parse(request.collectionId), request.itemsPerPage, request.selectedPage);
        }
    }
}
