using WatchmanDevotional.Models;

namespace WatchmanDevotional.Interfaces
{
    public interface IDevotionalRepository
    {
        Task<Devotionals?> GetByDateAsync(DateTime date);
        Task<IEnumerable<Devotionals>> GetRecentAsync(int count);
        Task<IEnumerable<Devotionals>> GetAllPastAsync();
       
        Task<IEnumerable<Devotionals>> GetAllAsync();
        Task<IEnumerable<Devotionals>> SearchAsync(string searchTerm);
        Task AddAsync(Devotionals devotionals);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateAsync(Guid id, Devotionals updatedData);
    }
}
