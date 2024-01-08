using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.IdentityModel.Tokens;
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
    public class GoalRepository : IGoalRepository
    {
        private readonly AppDBContext _context;

        public GoalRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task AddToCategory(string goalId, string categoryId)
        {
            await _context.CategoriesGoals.AddAsync(new CategoryGoal { CategoryId = Guid.Parse(goalId), GoalId = Guid.Parse(categoryId) });
        }

        public async Task<Goal> CreateAsync(Goal entity)
        {
            var result = await _context.AddAsync(entity);
            return result.Entity;
        }

        public async Task<Goal> DeleteAsync(string id)
        {
            var goal = await _context.Goals.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (goal == null)
                throw new Exception("No such goal is found.");

            _context.Goals.Remove(goal);

            return goal;
        }

        public async Task<IEnumerable<Goal>> GetAllAsync(Expression<Func<Goal, bool>> expression = null)
        {
            List<Goal> goals;

            if(expression != null)
                goals = await _context.Goals.Where(expression).Include(x => x.GoalCategories).ThenInclude(x => x.Category).AsNoTracking().ToListAsync();
            else
                goals = await _context.Goals.Include(x => x.GoalCategories).ThenInclude(x => x.Category).AsNoTracking().ToListAsync();

            if (goals.Count == 0)
                throw new Exception("No goals for now.");

            return goals;
        }

        public async Task<Goal> GetByIdAsync(string id)
        {
            var goal = await _context.Goals.Include(x=>x.Attachments).Include(x=>x.GoalCategories).ThenInclude(x=>x.Category).AsNoTracking().SingleOrDefaultAsync(x=>x.Id == Guid.Parse(id));

            if (goal == null)
                throw new Exception("No such goal is found.");

            return goal;
        }

        public void RemoveFromCategory(string goalId, string categoryId)
        {
            _context.CategoriesGoals.Remove(new CategoryGoal { CategoryId = Guid.Parse(goalId), GoalId = Guid.Parse(categoryId) });
        }

        public async Task<Goal> UpdateAsync(string id, Goal entity)
        {
            if (await _context.Goals.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id)) == null)
                throw new Exception("No such goal is found.");

            entity.Id = Guid.Parse(id);

            var result = _context.Update(entity);

            return result.Entity;
        }
    }
}
