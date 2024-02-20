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
    public class GoalNotificatorService : IGoalNotificatorService
    {
        private readonly IGoalNotificatorRepository _repo;
        private readonly IMapper _mapper;

        public GoalNotificatorService(IGoalNotificatorRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IAPIResponse<GoalNotificator>> CreateAsync(GoalNotificatorModel model)
        {
            var result = await _repo.CreateAsync(_mapper.Map<GoalNotificator>(model));
            return new APIResponse<GoalNotificator>(result);
        }

        public async Task<IAPIResponse<GoalNotificator>> DeleteAsync(string id)
        {
            var result = await _repo.DeleteAsync(id);
            return new APIResponse<GoalNotificator>(result);
        }

        public async Task<IAPIResponse<IEnumerable<GoalNotificator>>> GetAllAsync(Expression<Func<GoalNotificator, bool>> expression = null)
        {
            return new APIResponse<IEnumerable<GoalNotificator>>(await _repo.GetAllAsync(null));
        }

        public async Task<IAPIResponse<GoalNotificator>> GetByIdAsync(string id)
        {
            return new APIResponse<GoalNotificator>(await _repo.GetByIdAsync(id));
        }

        public async Task<IAPIResponse<GoalNotificator>> UpdateAsync(string id, GoalNotificatorModel model)
        {
            return new APIResponse<GoalNotificator>(await _repo.UpdateAsync(id, _mapper.Map<GoalNotificator>(model)));
        }
    }
}
