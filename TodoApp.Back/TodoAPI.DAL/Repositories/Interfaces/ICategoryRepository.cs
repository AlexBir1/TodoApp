using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.DAL.Entities;

namespace TodoAPI.DAL.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
    }
}
