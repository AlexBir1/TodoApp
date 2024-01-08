using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.DAL.UOW.Interfaces;
using TodoAPI.Services.Implementations;
using TodoAPI.Services.Interfaces;

namespace TodoAPI.Services.UOW
{
    public class ServiceRepository : IServiceRepository
    {
        public IAccountService AccountService { get; private set; }

        public IAttachmentService AttachmentService { get; private set; }

        public ICategoryService CategoryService { get; private set; }

        public ICollectionService CollectionService { get; private set; }

        public IGoalService GoalService { get; private set; }

        public ServiceRepository(IDBRepository dBRepository, IConfiguration config, IMapper mapper)
        {
            AccountService = new AccountService(dBRepository, config);
            AttachmentService = new AttachmentService(dBRepository, mapper);
            CategoryService = new CategoryService(dBRepository, mapper);
            CollectionService = new CollectionService(dBRepository, mapper);
            GoalService = new GoalService(dBRepository, mapper);
        }
    }
}
