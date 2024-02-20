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
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly AppDBContext _context;

        public AttachmentRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Attachment> CreateAsync(Attachment entity)
        {
            var result = await _context.Attachments.AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Attachment> DeleteAsync(string id)
        {
            var attachment = await _context.Attachments.SingleOrDefaultAsync(x=>x.Id == Guid.Parse(id));

            if (attachment == null)
                throw new ArgumentException("No such attachment is found.");

            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();

            return attachment;
        }

        public async Task<IEnumerable<Attachment>> GetAllAsync()
        {
            return await _context.Attachments.ToListAsync();
        }

        public async Task<IEnumerable<Attachment>> GetAllAsync(Expression<Func<Attachment, bool>> expression = null)
        {
            List<Attachment> attachments;

            if (expression != null)
                attachments = await _context.Attachments.Where(expression).AsNoTracking().ToListAsync();
            else
                attachments = await _context.Attachments.AsNoTracking().ToListAsync();

            return attachments;
        }

        public async Task<Attachment> GetByIdAsync(string id)
        {
            var attachment = await _context.Attachments.SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (attachment == null)
                throw new Exception("No such attachment is found.");

            return attachment;
        }

        public async Task<Attachment> UpdateAsync(string id, Attachment entity)
        {
            var attachment = await _context.Attachments.SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (attachment == null)
                throw new ArgumentException("No such attachment is found.");

            entity.Id = Guid.Parse(id);

            var result = _context.Attachments.Update(entity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }
    }
}
