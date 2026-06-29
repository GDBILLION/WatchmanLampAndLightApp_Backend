using Microsoft.EntityFrameworkCore;
using WatchmanDevotional.Data;
using WatchmanDevotional.Interfaces;
using WatchmanDevotional.Models;

namespace WatchmanDevotional.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly WatchmanDevotionDbContext _context;
        public QuizRepository(WatchmanDevotionDbContext context)
        {
            _context = context;
        }
        public async Task AddEntryAsync(QuizEntry entry)
        {
            await _context.QuizEntries.AddAsync(entry);
            await _context.SaveChangesAsync();
        }

        public async Task<List<QuizEntry>> GetCorrectEntriesForTodayAsync()
        {
            var today = DateTime.UtcNow.AddHours(1).Date;

            return await _context.QuizEntries
                .Where(x => x.IsCorrect &&
                          x.SubmittedAt.Date == today)
                .ToListAsync();
        }

        public async Task<IEnumerable<QuizEntry>> GetRandomWinnersAsync(int count)
        {
            var todayStart = DateTime.UtcNow.AddHours(1).Date;
            var tomorrowStart = todayStart.AddDays(1);
            

            var winners = await _context.QuizEntries
                 .Where(x => x.IsCorrect &&
                            x.SubmittedAt >= todayStart &&
                            x.SubmittedAt < tomorrowStart)
                .OrderBy(x => EF.Functions.Random())
                .Take(count)
                .ToListAsync();

            if (winners.Any())
            {
                foreach (var winner in winners)
                {
                    winner.WasSelelectedAsWinner = true;
                    winner.WinningDate = DateTime.UtcNow.AddHours(1);
                }
                await _context.SaveChangesAsync();
            }
            return winners;
        }

        public async Task<bool> HasAlreadySubmittedTodayAsync(string phoneNumber)
        {
            var today = DateTime.UtcNow.AddHours(1).Date;

            return await _context.QuizEntries
                .AnyAsync(x => x.PhoneNumber == phoneNumber &&
                              x.SubmittedAt.Date == today);
        }

        public async Task<List<QuizEntry>> GetTodayAllEntriesAsync()
        {
            var today = DateTime.UtcNow.AddHours(1).Date;

            return await _context.QuizEntries
                .Where(x => x.SubmittedAt.Date == today)
                .OrderByDescending(x => x.SubmittedAt)
                .ToListAsync();
        }

        // 🌟 NEW FOR PHASE 1: MANUALLY GRADE AN ENTRY
        public async Task<bool> UpdateEntryGradeAsync(Guid id, bool isCorrect)
        {
            var entry = await _context.QuizEntries.FindAsync(id);
            if (entry == null) return false;

            entry.IsCorrect = isCorrect;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
