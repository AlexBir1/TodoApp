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
    public class GoalNotificatorRepository : IGoalNotificatorRepository
    {
        private readonly AppDBContext _context;

        public GoalNotificatorRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<GoalNotificator> CreateAsync(GoalNotificator entity)
        {
            var result = await _context.GoalNotificators.AddAsync(entity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<GoalNotificator> DeleteAsync(string id)
        {
            var entity = await _context.GoalNotificators.AsNoTracking().SingleOrDefaultAsync(x => x.GoalId == Guid.Parse(id));

            if (entity != null)
            {
                var result = _context.GoalNotificators.Remove(entity);
                await _context.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<IEnumerable<GoalNotificator>> GetAllAsync(Expression<Func<GoalNotificator, bool>> expression = null)
        {
            if (expression != null)
                return await _context.GoalNotificators.Where(expression).ToListAsync();
            else
                return await _context.GoalNotificators.ToListAsync();
        }

        public async Task<GoalNotificator> GetByIdAsync(string id)
        {
            return await _context.GoalNotificators.SingleOrDefaultAsync(x => x.GoalId == Guid.Parse(id));
        }

        public async Task<GoalNotificator> UpdateAsync(string id, GoalNotificator entity)
        {
            var result = _context.GoalNotificators.Update(entity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }
    }
}
