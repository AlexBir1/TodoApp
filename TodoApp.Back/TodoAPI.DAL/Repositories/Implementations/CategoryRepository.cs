using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.DAL.DBContext;
using TodoAPI.DAL.Entities;
using TodoAPI.DAL.Repositories.Interfaces;

namespace TodoAPI.DAL.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDBContext _context;

        public CategoryRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateAsync(Category entity)
        {
            var result = await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Category> DeleteAsync(string id)
        {
            var category = await _context.Categories.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if(category == null)
                throw new ArgumentException("No such category is found.");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync();

            if(categories.Count == 0)
                throw new Exception("No categories for now.");

            return categories;
        }

        public async Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>> expression = null)
        {
            List<Category> categories;

            if (expression != null)
                categories = await _context.Categories.Where(expression).AsNoTracking().ToListAsync();
            else
                categories = await _context.Categories.AsNoTracking().ToListAsync();

            return categories;
        }

        public async Task<Category> GetByIdAsync(string id)
        {
            var category = await _context.Categories.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (category == null)
                throw new ArgumentException("No such category is found.");

            return category;
        }

        public async Task<Category> UpdateAsync(string id, Category entity)
        {
            if(await _context.Categories.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id)) == null)
                throw new ArgumentException("No such category is found.");

            entity.Id = Guid.Parse(id);

            var result = _context.Update(entity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }
    }
}
