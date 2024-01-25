using MediatR;
using Microsoft.AspNetCore.SignalR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.Services.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.Hubs;
using TodoAPI.CQRS.Commands.Goals;

namespace TodoAPI.CQRS.Handlers.Goals
{
    public class CreateGoalHandler : IRequestHandler<CreateGoalCommand, IAPIResponse<Goal>>
    {
        private readonly IGoalService _goalService;
        private readonly IHubContext<NotificationHub> _notifier;

        public CreateGoalHandler(IGoalService goalService)
        {
            _goalService = goalService;
        }

        public async Task<IAPIResponse<Goal>> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
        {
            return await _goalService.CreateAsync(request.model);
        }
    }
}
