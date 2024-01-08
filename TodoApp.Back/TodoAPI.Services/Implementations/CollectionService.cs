using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.APIResponse.Implementations;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.DAL.UOW.Interfaces;
using TodoAPI.Services.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.Services.Implementations
{
    public class CollectionService : ICollectionService
    {
        private readonly IDBRepository _dbRepo;
        private readonly IMapper _mapper;

        public CollectionService(IDBRepository dbRepo, IMapper mapper)
        {
            _dbRepo = dbRepo;
            _mapper = mapper;
        }

        public async Task<IAPIResponse<Collection>> CreateAsync(CollectionModel model)
        {
            var result = await _dbRepo.CollectionRepo.CreateAsync(_mapper.Map<Collection>(model));
            await _dbRepo.CommitAsync();
            return new APIResponse<Collection>(true, result);
        }

        public async Task<IAPIResponse<Collection>> DeleteAsync(string id)
        {
            var result = await _dbRepo.CollectionRepo.DeleteAsync(id);
            await _dbRepo.CommitAsync();
            return new APIResponse<Collection>(true, result);
        }

        public async Task<IAPIResponse<IEnumerable<Collection>>> GetAllAsync(Expression<Func<Collection, bool>> expression = null)
        {
            if (expression != null)
                return new APIResponse<IEnumerable<Collection>>(true, await _dbRepo.CollectionRepo.GetAllAsync(expression));
            else
                return new APIResponse<IEnumerable<Collection>>(true, await _dbRepo.CollectionRepo.GetAllAsync());
        }

        public async Task<IAPIResponse<Collection>> GetByIdAsync(string id)
        {
            return new APIResponse<Collection>(true, await _dbRepo.CollectionRepo.GetByIdAsync(id));
        }

        public async Task<IAPIResponse<Collection>> UpdateAsync(string id, CollectionModel model)
        {
            var result = await _dbRepo.CollectionRepo.UpdateAsync(id, _mapper.Map<Collection>(model));
            await _dbRepo.CommitAsync();
            return new APIResponse<Collection>(true, result);
        }
    }
}
