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
    public class GoalService : IGoalService
    {
        private readonly IDBRepository _dbRepo;
        private readonly IMapper _mapper;

        public GoalService(IDBRepository dbRepo, IMapper mapper)
        {
            _dbRepo = dbRepo;
            _mapper = mapper;
        }

        public async Task<CategoryGoal> AddToCategory(string goalId, string categoryId)
        {
            await _dbRepo.GoalRepo.AddToCategory(goalId, categoryId);
            await _dbRepo.CommitAsync();
            return new CategoryGoal { GoalId = Guid.Parse(goalId), CategoryId = Guid.Parse(categoryId) };
        }

        public async Task<IAPIResponse<Goal>> CreateAsync(GoalModel model)
        {
            var result = await _dbRepo.GoalRepo.CreateAsync(_mapper.Map<Goal>(model));
            await _dbRepo.CommitAsync();
            return new APIResponse<Goal>(true, result);
        }

        public async Task<IAPIResponse<Goal>> DeleteAsync(string id)
        {
            var result = await _dbRepo.GoalRepo.DeleteAsync(id);
            await _dbRepo.CommitAsync();
            return new APIResponse<Goal>(true, result);
        }

        public async Task<IAPIResponse<IEnumerable<Goal>>> GetAllAsync(Expression<Func<Goal, bool>> expression = null)
        {
            if (expression != null)
                return new APIResponse<IEnumerable<Goal>>(true, await _dbRepo.GoalRepo.GetAllAsync(expression));
            else
                return new APIResponse<IEnumerable<Goal>>(true, await _dbRepo.GoalRepo.GetAllAsync());
        }

        public async Task<IAPIResponse<Goal>> GetByIdAsync(string id)
        {
            return new APIResponse<Goal>(true, await _dbRepo.GoalRepo.GetByIdAsync(id));
        }

        public async Task<CategoryGoal> RemoveFromCategory(string goalId, string categoryId)
        {
            _dbRepo.GoalRepo.RemoveFromCategory(goalId, categoryId);
            await _dbRepo.CommitAsync();
            return new CategoryGoal { GoalId = Guid.Parse(goalId), CategoryId = Guid.Parse(categoryId) };
        }

        public async Task<IAPIResponse<Goal>> UpdateAsync(string id, GoalModel model)
        {
            var result = await _dbRepo.GoalRepo.UpdateAsync(id, _mapper.Map<Goal>(model));
            await _dbRepo.CommitAsync();
            return new APIResponse<Goal>(true, await _dbRepo.GoalRepo.GetByIdAsync(id));
        }
    }
}
