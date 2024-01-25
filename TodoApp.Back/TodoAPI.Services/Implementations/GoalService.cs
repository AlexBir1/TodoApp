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
    public class GoalService : IGoalService
    {
        private readonly IGoalRepository _goalRepo;
        private readonly IMapper _mapper;

        public GoalService(IMapper mapper, IGoalRepository goalRepo)
        {
            _mapper = mapper;
            _goalRepo = goalRepo;
        }

        public async Task<IAPIResponse<CategoryGoal>> AddToCategory(string goalId, string categoryId)
        {
            await _goalRepo.AddToCategory(goalId, categoryId);
            return new APIResponse<CategoryGoal>(true, new CategoryGoal { GoalId = Guid.Parse(goalId), CategoryId = Guid.Parse(categoryId) });
        }

        public async Task<IAPIResponse<Goal>> CreateAsync(GoalModel model)
        {
            var result = await _goalRepo.CreateAsync(_mapper.Map<Goal>(model));
            return new APIResponse<Goal>(true, result);
        }

        public async Task<IAPIResponse<Goal>> DeleteAsync(string id)
        {
            var result = await _goalRepo.DeleteAsync(id);
            return new APIResponse<Goal>(true, result);
        }

        public async Task<IAPIResponse<IEnumerable<Goal>>> GetAllPagedAsync(Expression<Func<Goal, bool>> expression = null, int itemsPerPage = 1, int selectedPage = 1)
        {
            if (expression != null)
            {
                var result = await _goalRepo.GetAllPagedAsync(expression, itemsPerPage, selectedPage);
                return new APIResponse<IEnumerable<Goal>>(true, result.Items, result.ItemsCount, result.SelectedPage, result.ItemsPerPage);
            }
            else
            {
                var result = await _goalRepo.GetAllPagedAsync(itemsPerPage: itemsPerPage, selectedPage: selectedPage);
                return new APIResponse<IEnumerable<Goal>>(true, result.Items, result.ItemsCount, result.SelectedPage, result.ItemsPerPage);
            }
        }

        public Task<IAPIResponse<IEnumerable<Goal>>> GetAllAsync(Expression<Func<Goal, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IAPIResponse<Goal>> GetByIdAsync(string id)
        {
            return new APIResponse<Goal>(true, await _goalRepo.GetByIdAsync(id));
        }

        public async Task<IAPIResponse<CategoryGoal>> RemoveFromCategory(string goalId, string categoryId)
        {
            await _goalRepo.RemoveFromCategory(goalId, categoryId);
            return new APIResponse<CategoryGoal>(true, new CategoryGoal { GoalId = Guid.Parse(goalId), CategoryId = Guid.Parse(categoryId) });
        }

        public async Task<IAPIResponse<Goal>> UpdateAsync(string id, GoalModel model)
        {
            var result = await _goalRepo.UpdateAsync(id, _mapper.Map<Goal>(model));
            return new APIResponse<Goal>(true, await _goalRepo.GetByIdAsync(id));
        }
    }
}
