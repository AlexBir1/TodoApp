using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Goals;
using TodoAPI.DAL.Entities;
using TodoAPI.Services.Interfaces;

namespace TodoAPI.CQRS.Handlers.Goals
{
    public class AddGoalToCategoryHandler : IRequestHandler<AddGoalToCategoryCommand, IAPIResponse<CategoryGoal>>
    {
        private readonly IGoalService _goalService;

        public AddGoalToCategoryHandler(IGoalService goalService)
        {
            _goalService = goalService;
        }

        public async Task<IAPIResponse<CategoryGoal>> Handle(AddGoalToCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _goalService.AddToCategory(request.goalId, request.categoryId);
        }
    }
}
