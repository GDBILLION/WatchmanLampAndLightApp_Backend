using Microsoft.EntityFrameworkCore;
using WatchmanDevotional.Data;
using WatchmanDevotional.Interfaces;
using WatchmanDevotional.Models;

namespace WatchmanDevotional.Repositories
{
    public class DevotionalRepository : IDevotionalRepository
    {
        private readonly WatchmanDevotionDbContext _context;
        public DevotionalRepository(WatchmanDevotionDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Devotionals devotionals)
        {
            await _context.AddAsync(devotionals);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Devotionals>> GetAllPastAsync()
        {
            var lagosToday = DateTime.UtcNow.AddHours(1).Date;

            return await _context.Devotionals
                .Where(d => d.PublishDate.Date < lagosToday)
                .OrderByDescending(d => d.PublishDate)
                .ToListAsync();
        }

        public async Task<Devotionals?> GetByDateAsync(DateTime date)
        {
            
            var targetDate = date.Date;

            return await _context.Devotionals
                .FirstOrDefaultAsync(d => d.PublishDate.Date == targetDate);
        }

        public async Task<IEnumerable<Devotionals>> GetRecentAsync(int count)
        {
            return await _context.Devotionals
                .OrderByDescending(d => d.PublishDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Devotionals>> GetAllAsync()
        {
            return await _context.Devotionals.OrderByDescending(d => d.PublishDate).ToListAsync();
        }

        public async Task<IEnumerable<Devotionals>> SearchAsync(string searchTerm)
        {
            return await _context.Devotionals
                .Where(d => EF.Functions.ILike(d.Title, $"%{searchTerm}%") ||
                EF.Functions.ILike(d.Content, $"%{searchTerm}%"))
                .OrderByDescending(d => d.PublishDate)
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var devotional = await _context.Devotionals.FindAsync(id);
            if (devotional == null) return false;

            _context.Devotionals.Remove(devotional);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Guid id, Devotionals updatedData)
        {
            var existing = await _context.Devotionals.FindAsync(id);
            if (existing == null) return false;

            // Map changed properties over to tracked entity
            existing.Title = updatedData.Title;
            existing.Content = updatedData.Content;
            existing.ScriptureReference = updatedData.ScriptureReference;
            existing.MemoryVerse = updatedData.MemoryVerse;
            existing.YoutubeVideoId = updatedData.YoutubeVideoId;
            existing.PublishDate = updatedData.PublishDate;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
