using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.DAL.Entities;

namespace TodoAPI.DAL.Repositories.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<bool> CheckPasswordAsync(Account account, string password);
        bool SetPassword(Account account, string password);
    }
}
