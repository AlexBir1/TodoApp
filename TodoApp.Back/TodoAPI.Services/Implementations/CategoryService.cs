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
using TodoAPI.DAL.Repositories.Interfaces;
using TodoAPI.Services.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, ICategoryRepository categoryRepo)
        {
            _mapper = mapper;
            _categoryRepo = categoryRepo;
        }

        public async Task<IAPIResponse<Category>> CreateAsync(CategoryModel model)
        {
            var result = await _categoryRepo.CreateAsync(_mapper.Map<Category>(model));
            return new APIResponse<Category>(true, result);
        }

        public async Task<IAPIResponse<Category>> DeleteAsync(string id)
        {
            var result = await _categoryRepo.DeleteAsync(id);
            return new APIResponse<Category>(true, result);
        }

        public async Task<IAPIResponse<IEnumerable<Category>>> GetAllAsync(Expression<Func<Category, bool>> expression = null)
        {
            if (expression != null)
            {
                return new APIResponse<IEnumerable<Category>>(true, await _categoryRepo.GetAllAsync(expression));
            }
            else
            {
                return new APIResponse<IEnumerable<Category>>(true, await _categoryRepo.GetAllAsync());
            } 
        }

        public async Task<IAPIResponse<Category>> GetByIdAsync(string id)
        {
            return new APIResponse<Category>(true, await _categoryRepo.GetByIdAsync(id));
        }

        public async Task<IAPIResponse<Category>> UpdateAsync(string id, CategoryModel model)
        {
            var result = await _categoryRepo.UpdateAsync(id, _mapper.Map<Category>(model));
            return new APIResponse<Category>(true, result);
        }
    }
}
