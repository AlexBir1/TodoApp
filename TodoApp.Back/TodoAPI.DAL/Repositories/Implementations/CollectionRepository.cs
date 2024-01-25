
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TodoAPI.DAL.DBContext;
using TodoAPI.DAL.Entities;
using TodoAPI.DAL.Repositories.Interfaces;

namespace TodoAPI.DAL.Repositories.Implementations
{
    public class CollectionRepository : ICollectionRepository
    {
        private readonly AppDBContext _context;

        public CollectionRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Collection> CreateAsync(Collection entity)
        {
            var result = await _context.Collections.AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Collection> DeleteAsync(string id)
        {
            var collection = await _context.Collections.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (collection == null)
            {
                var qa = await _context.Collections.ToListAsync();
                throw new Exception("No such collection is found.");
            }
                

            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();

            return collection;
        }

        public async Task<IEnumerable<Collection>> GetAllAsync(Expression<Func<Collection, bool>> expression = null)
        {
            List<Collection> collections;
            
            if (expression != null)
                collections = await _context.Collections.Where(expression).AsNoTracking().ToListAsync();
            else
                collections = await _context.Collections.AsNoTracking().ToListAsync();

            if (collections.Count == 0)
                throw new Exception("No collections for now.");

            return collections;
        }

        public async Task<Collection> GetByIdAsync(string id)
        {
            var collection = await _context.Collections.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (collection == null)
                throw new Exception("No such collection is found.");

            return collection;
        }

        public async Task<Collection> UpdateAsync(string id, Collection entity)
        {
            var collection = await _context.Collections.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (collection == null)
                throw new Exception("No such collection is found.");

            entity.Id = Guid.Parse(id);

            var result = _context.Collections.Update(entity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }
    }
}
