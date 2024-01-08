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
    public class CategoryService : ICategoryService
    {
        private readonly IDBRepository _dbRepo;
        private readonly IMapper _mapper;

        public CategoryService(IDBRepository dbRepo, IMapper mapper)
        {
            _dbRepo = dbRepo;
            _mapper = mapper;
        }

        public async Task<IAPIResponse<Category>> CreateAsync(CategoryModel model)
        {
            var result = await _dbRepo.CategoryRepo.CreateAsync(_mapper.Map<Category>(model));
            await _dbRepo.CommitAsync();
            return new APIResponse<Category>(true, result);
        }

        public async Task<IAPIResponse<Category>> DeleteAsync(string id)
        {
            var result = await _dbRepo.CategoryRepo.DeleteAsync(id);
            await _dbRepo.CommitAsync();
            return new APIResponse<Category>(true, result);
        }

        public async Task<IAPIResponse<IEnumerable<Category>>> GetAllAsync(Expression<Func<Category, bool>> expression = null)
        {
            if (expression != null)
                return new APIResponse<IEnumerable<Category>>(true, await _dbRepo.CategoryRepo.GetAllAsync(expression));
            else
                return new APIResponse<IEnumerable<Category>>(true, await _dbRepo.CategoryRepo.GetAllAsync());
        }

        public async Task<IAPIResponse<Category>> GetByIdAsync(string id)
        {
            return new APIResponse<Category>(true, await _dbRepo.CategoryRepo.GetByIdAsync(id));
        }

        public async Task<IAPIResponse<Category>> UpdateAsync(string id, CategoryModel model)
        {
            var result = await _dbRepo.CategoryRepo.UpdateAsync(id, _mapper.Map<Category>(model));
            await _dbRepo.CommitAsync();
            return new APIResponse<Category>(true, result);
        }
    }
}
