
using System.Linq.Expressions;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.Shared.Models;

namespace TodoAPI.Services.Interfaces
{
    public interface IGoalService : IService<Goal, GoalModel>
    {
        Task<IAPIResponse<CategoryGoal>> AddToCategory(string goalId, string categoryId);
        Task<IAPIResponse<CategoryGoal>> RemoveFromCategory(string goalId, string categoryId);
        Task<IAPIResponse<IEnumerable<Goal>>> GetAllPagedAsync(Expression<Func<Goal, bool>> expression = null, int itemsPerPage = 1, int selectedPage = 1);
    }
}
