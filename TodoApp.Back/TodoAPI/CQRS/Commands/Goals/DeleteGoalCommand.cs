using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;

namespace TodoAPI.CQRS.Commands.Goals
{
    public record DeleteGoalCommand(string goalId) : IRequest<IAPIResponse<Goal>>;
}
