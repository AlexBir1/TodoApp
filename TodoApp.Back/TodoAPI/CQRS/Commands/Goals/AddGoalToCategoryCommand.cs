using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;

namespace TodoAPI.CQRS.Commands.Goals
{
    public record AddGoalToCategoryCommand(string goalId, string categoryId) : IRequest<IAPIResponse<CategoryGoal>>;
}
