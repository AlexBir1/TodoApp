
using TodoAPI.DAL.Entities;
using TodoAPI.Shared.Models;

namespace TodoAPI.Services.Interfaces
{
    public interface IGoalService : IService<Goal, GoalModel>
    {
        Task<Goal> AddToCategory(string goalId, string categoryId);
        Task<Goal> RemoveFromCategory(string goalId, string categoryId);
    }
}
