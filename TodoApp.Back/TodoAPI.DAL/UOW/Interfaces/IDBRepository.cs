using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.DAL.Repositories.Interfaces;

namespace TodoAPI.DAL.UOW.Interfaces
{
    public interface IDBRepository
    {
        IAccountRepository AccountRepo { get; }
        IAttachmentRepository AttachmentRepo { get; }
        ICategoryRepository CategoryRepo { get; }
        ICollectionRepository CollectionRepo { get; }
        IGoalRepository GoalRepo { get; }
        Task CommitAsync();
    }
}
