using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.APIResponse.Interfaces;

namespace TodoAPI.Services.Interfaces
{
    public interface IService<TEntity, TModel>
    {
        Task<IAPIResponse<TEntity>> CreateAsync(TModel model);
        Task<IAPIResponse<TEntity>> DeleteAsync(string id);
        Task<IAPIResponse<TEntity>> UpdateAsync(string id, TModel model);
        Task<IAPIResponse<TEntity>> GetByIdAsync(string id);
        Task<IAPIResponse<IEnumerable<TEntity>>> GetAllAsync(Expression<Func<TEntity, bool>> expression = null);
    }
}
