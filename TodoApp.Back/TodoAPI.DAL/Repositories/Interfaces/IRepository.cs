using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
