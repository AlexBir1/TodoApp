using AutoMapper;
using System.Linq.Expressions;
using TodoAPI.APIResponse.Implementations;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.DAL.Repositories.Interfaces;
using TodoAPI.Services.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.Services.Implementations
{
    public class CollectionService : ICollectionService
    {
        private readonly ICollectionRepository _collectionRepo;
        private readonly IMapper _mapper;

        public CollectionService(IMapper mapper, ICollectionRepository collectionRepo)
        {
            _mapper = mapper;
            _collectionRepo = collectionRepo;
        }

        public async Task<IAPIResponse<Collection>> CreateAsync(CollectionModel model)
        {
            var result = await _collectionRepo.CreateAsync(_mapper.Map<Collection>(model));
            return new APIResponse<Collection>(true, result);
        }

        public async Task<IAPIResponse<Collection>> DeleteAsync(string id)
        {
            var result = await _collectionRepo.DeleteAsync(id);
            return new APIResponse<Collection>(true, result);
        }

        public async Task<IAPIResponse<IEnumerable<Collection>>> GetAllAsync(Expression<Func<Collection, bool>> expression = null)
        {
            if (expression != null)
            {
                return new APIResponse<IEnumerable<Collection>>(true, await _collectionRepo.GetAllAsync(expression));
            }
            else
            {
                return new APIResponse<IEnumerable<Collection>>(true, await _collectionRepo.GetAllAsync());
            }
        }

        public async Task<IAPIResponse<Collection>> GetByIdAsync(string id)
        {
            return new APIResponse<Collection>(true, await _collectionRepo.GetByIdAsync(id));
        }

        public async Task<IAPIResponse<Collection>> UpdateAsync(string id, CollectionModel model)
        {
            var result = await _collectionRepo.UpdateAsync(id, _mapper.Map<Collection>(model));
            return new APIResponse<Collection>(true, result);
        }
    }
}
