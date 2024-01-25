using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;

namespace TodoAPI.CQRS.Commands.Goals
{
    public record RemoveGoalFromCategoryCommand(string goalId, string categoryId) : IRequest<IAPIResponse<CategoryGoal>>;
}
