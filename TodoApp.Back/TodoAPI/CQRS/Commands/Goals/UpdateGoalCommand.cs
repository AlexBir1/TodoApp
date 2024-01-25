using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.Shared.Models;

namespace TodoAPI.CQRS.Commands.Goals
{
    public record UpdateGoalCommand(string goalId, GoalModel model) : IRequest<IAPIResponse<Goal>>;
}
