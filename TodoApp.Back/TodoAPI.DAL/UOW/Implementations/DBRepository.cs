using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.DAL.DBContext;
using TodoAPI.DAL.Entities;
using TodoAPI.DAL.Repositories.Implementations;
using TodoAPI.DAL.Repositories.Interfaces;
using TodoAPI.DAL.UOW.Interfaces;

namespace TodoAPI.DAL.UOW.Implementations
{
    public class DBRepository : IDBRepository
    {
        private AppDBContext _context;

        public IAccountRepository AccountRepo { get; private set; }

        public IAttachmentRepository AttachmentRepo { get; private set; }

        public ICategoryRepository CategoryRepo { get; private set; }

        public ICollectionRepository CollectionRepo { get; private set; }

        public IGoalRepository GoalRepo { get; private set; }

        public DBRepository(AppDBContext context, UserManager<Account> userManager)
        {
            _context = context;
            AccountRepo = new AccountRepository(userManager);
            AttachmentRepo = new AttachmentRepository(context);
            CategoryRepo = new CategoryRepository(context);
            GoalRepo = new GoalRepository(context);
            CollectionRepo = new CollectionRepository(context);
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
