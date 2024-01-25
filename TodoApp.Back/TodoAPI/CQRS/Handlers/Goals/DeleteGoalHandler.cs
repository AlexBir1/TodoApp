using MediatR;
using Microsoft.AspNetCore.SignalR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Goals;
using TodoAPI.DAL.Entities;
using TodoAPI.Hubs;
using TodoAPI.Services.Interfaces;

namespace TodoAPI.CQRS.Handlers.Goals
{
    public class DeleteGoalHandler : IRequestHandler<DeleteGoalCommand, IAPIResponse<Goal>>
    {
        private readonly IGoalService _goalService;

        public DeleteGoalHandler(IGoalService goalService)
        {
            _goalService = goalService;
        }

        public async Task<IAPIResponse<Goal>> Handle(DeleteGoalCommand request, CancellationToken cancellationToken)
        {
            return await _goalService.DeleteAsync(request.goalId);
        }
    }
}
