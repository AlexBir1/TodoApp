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
            await _context.CategoriesGoals.AddAsync(new CategoryGoal { CategoryId = Guid.Parse(categoryId), GoalId = Guid.Parse(goalId) });
            await _context.SaveChangesAsync();
        }

        public async Task<Goal> CreateAsync(Goal entity)
        {
            var result = await _context.Goals.AddAsync(entity);
            await _context.SaveChangesAsync();

            _context.Entry(result.Entity).Reference(x => x.Collection).Load();

            result.Entity.OptimizeDepth();

            return result.Entity;
        }

        public async Task<Goal> DeleteAsync(string id)
        {
            var goal = await _context.Goals.Include(x => x.GoalCategories).AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (goal == null)
                throw new ArgumentException("No such goal is found.");

            _context.Goals.Remove(goal);
            await _context.SaveChangesAsync();

            return goal;
        }

        public async Task<PagedResult<Goal>> GetAllPagedAsync(Expression<Func<Goal, bool>> expression = null, int itemsPerPage = 1, int selectedPage = 1)
        {
            List<Goal> goals;
            int dbItemsCount = 0;
            int countToSkip = selectedPage > 1 ? (selectedPage - 1) * itemsPerPage : 0;

            if (expression != null)
            {
                goals = await _context.Goals.Where(expression).Include(x=>x.Collection).Include(x => x.Attachments).Include(x => x.GoalCategories).ThenInclude(x => x.Category).Skip(countToSkip).Take(itemsPerPage).AsNoTracking().ToListAsync();
                dbItemsCount = await _context.Goals.Where(expression).CountAsync();
            }
            else
            {
                goals = await _context.Goals.Include(x => x.Collection).Include(x => x.Attachments).Include(x => x.GoalCategories).ThenInclude(x => x.Category).Skip(countToSkip).Take(itemsPerPage).AsNoTracking().ToListAsync();
                dbItemsCount = await _context.Goals.CountAsync();
            }

            foreach (var goal in goals)
            {
                goal.OptimizeDepth();
            }

            return new PagedResult<Goal>(goals, dbItemsCount, selectedPage, itemsPerPage);
        }

        public async Task<IEnumerable<Goal>> GetAllAsync(Expression<Func<Goal, bool>> expression = null)
        {
            List<Goal> goals;

            if (expression != null)
                goals = await _context.Goals.Where(expression).Include(x => x.Attachments).Include(x => x.GoalCategories).ThenInclude(x => x.Category).AsNoTracking().ToListAsync();
            else
                goals = await _context.Goals.Include(x => x.GoalCategories).ThenInclude(x => x.Category).AsNoTracking().ToListAsync();

            foreach (var goal in goals)
            {
                goal.OptimizeDepth();
            }

            return goals;
        }

        public async Task<Goal> GetByIdAsync(string id)
        {
            var goal = await _context.Goals.Include(x => x.Collection).Include(x => x.Attachments).Include(x => x.GoalCategories).ThenInclude(x => x.Category).AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (goal == null)
                throw new Exception("No such goal is found.");

            foreach(var a in goal.Attachments)
            {
                a.Goal = null;
            }
            foreach (var gc in goal.GoalCategories)
            {
                gc.Goal = null;
                gc.Category.CategoryGoals = null;
            }

            goal.OptimizeDepth();

            return goal;
        }

        public async Task RemoveFromCategory(string goalId, string categoryId)
        {
            _context.CategoriesGoals.Remove(new CategoryGoal { CategoryId = Guid.Parse(categoryId), GoalId = Guid.Parse(goalId) });
            await _context.SaveChangesAsync();
        }

        public async Task<Goal> UpdateAsync(string id, Goal entity)
        {
            var goal = await _context.Goals.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (goal == null)
                throw new Exception("No such goal is found.");

            entity.Id = Guid.Parse(id);
            entity.UpdateDate = DateTime.Now;

            _context.Goals.Update(entity);
            await _context.SaveChangesAsync();

            _context.Entry(entity).Reference(x => x.Collection).Load();
            entity.OptimizeDepth();

            return entity;
        }

        
    }
}
