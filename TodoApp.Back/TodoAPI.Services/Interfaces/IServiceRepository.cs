using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAPI.Services.Interfaces
{
    public interface IServiceRepository
    {
        IAccountService AccountService { get; }
        IAttachmentService AttachmentService { get; }
        ICategoryService CategoryService { get; }
        ICollectionService CollectionService { get; }
        IGoalService GoalService { get; }
    }
}
