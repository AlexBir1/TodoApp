using TodoAPI.DAL.Entities;

namespace TodoAPI.DAL.Repositories.Interfaces
{
    public interface IGoalRepository : IRepository<Goal>
    {
        Task AddToCategory(string goalId, string categoryId);
        void RemoveFromCategory(string goalId, string categoryId);
    }
}
