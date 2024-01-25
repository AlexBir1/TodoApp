using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Goals;
using TodoAPI.DAL.Entities;
using TodoAPI.Services.Interfaces;

namespace TodoAPI.CQRS.Handlers.Goals
{
    public class UpdateGoalHandler : IRequestHandler<UpdateGoalCommand, IAPIResponse<Goal>>
    {
        private readonly IGoalService _goalService;

        public UpdateGoalHandler(IGoalService goalService)
        {
            _goalService = goalService;
        }

        public async Task<IAPIResponse<Goal>> Handle(UpdateGoalCommand request, CancellationToken cancellationToken)
        {
            return await _goalService.UpdateAsync(request.goalId, request.model);
        }
    }
}
