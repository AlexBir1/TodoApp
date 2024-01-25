
using System.Linq.Expressions;


namespace TodoAPI.DAL.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(string id, T entity);
        Task<T> DeleteAsync(string id);
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>> expression = null);
    }
}
