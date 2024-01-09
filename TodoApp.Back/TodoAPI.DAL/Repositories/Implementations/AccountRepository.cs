using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;
using TodoAPI.DAL.DBContext;
using TodoAPI.DAL.Entities;
using TodoAPI.DAL.Repositories.Interfaces;

namespace TodoAPI.DAL.Repositories.Implementations
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<Account> _userManager;

        public AccountRepository(UserManager<Account> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CheckPasswordAsync(Account account, string password)
        {
            return await _userManager.CheckPasswordAsync(account, password);
        }

        public async Task<Account> CreateAsync(Account entity)
        {
            if (await _userManager.FindByNameAsync(entity.UserName) != null)
                throw new Exception("Account with this username exists.");

            if (await _userManager.FindByEmailAsync(entity.Email) != null)
                throw new Exception("Account with this email exists.");

            if (await _userManager.Users.SingleOrDefaultAsync(x=>x.PhoneNumber == entity.PhoneNumber) != null)
                throw new Exception("Account with this phone number exists.");

            await _userManager.CreateAsync(entity);

            return await _userManager.FindByNameAsync(entity.UserName);
        }

        public async Task<Account> DeleteAsync(string id)
        {
            var account = await _userManager.FindByIdAsync(id);

            if (account == null)
                throw new Exception("No such account is found.");

            await _userManager.DeleteAsync(account);

            return account;
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IEnumerable<Account>> GetAllAsync(Expression<Func<Account, bool>> expression)
        {
            return await _userManager.Users.Where(expression).ToListAsync();
        }

        public async Task<Account> GetByIdAsync(string id)
        {
            var account = await _userManager.FindByIdAsync(id);

            if (account == null)
                throw new Exception("No such account is found.");

            return account;
        }

        public async Task<bool> SetPassword(Account account, string password)
        {
            var result = await _userManager.AddPasswordAsync(account, password);
            return result.Succeeded;
        }

        public async Task<Account> UpdateAsync(string id, Account entity)
        {
            throw new NotImplementedException();
        }
    }
}
