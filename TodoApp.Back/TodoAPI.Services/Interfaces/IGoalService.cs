
using TodoAPI.DAL.Entities;
using TodoAPI.Shared.Models;

namespace TodoAPI.Services.Interfaces
{
    public interface IGoalService : IService<Goal, GoalModel>
    {
        Task<CategoryGoal> AddToCategory(string goalId, string categoryId);
        Task<CategoryGoal> RemoveFromCategory(string goalId, string categoryId);
    }
}
