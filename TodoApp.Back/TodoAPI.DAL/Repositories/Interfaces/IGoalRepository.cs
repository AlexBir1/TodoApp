using System.Linq.Expressions;
using TodoAPI.DAL.Entities;

namespace TodoAPI.DAL.Repositories.Interfaces
{
    public interface IGoalRepository : IRepository<Goal>
    {
        Task AddToCategory(string goalId, string categoryId);
        Task RemoveFromCategory(string goalId, string categoryId);
        Task<PagedResult<Goal>> GetAllPagedAsync(Expression<Func<Goal, bool>> expression = null, int itemsPerPage = 1, int selectedPage = 1);
    }
}
