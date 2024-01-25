using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Goals;
using TodoAPI.DAL.Entities;
using TodoAPI.Services.Interfaces;

namespace TodoAPI.CQRS.Handlers.Goals
{
    public class RemoveGoalFromCategoryHandler : IRequestHandler<RemoveGoalFromCategoryCommand, IAPIResponse<CategoryGoal>>
    {
        private readonly IGoalService _goalService;

        public RemoveGoalFromCategoryHandler(IGoalService goalService)
        {
            _goalService = goalService;
        }

        public async Task<IAPIResponse<CategoryGoal>> Handle(RemoveGoalFromCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _goalService.RemoveFromCategory(request.goalId, request.categoryId);
        }
    }
}
