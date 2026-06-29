using WatchmanDevotional.Models;

namespace WatchmanDevotional.Interfaces
{
    public interface IQuizRepository
    {
        Task AddEntryAsync(QuizEntry entry);
        Task<bool> HasAlreadySubmittedTodayAsync(string phoneNumber);
        Task<List<QuizEntry>> GetCorrectEntriesForTodayAsync();
        Task<IEnumerable<QuizEntry>> GetRandomWinnersAsync(int count);
        Task<List<QuizEntry>> GetTodayAllEntriesAsync();
        Task<bool> UpdateEntryGradeAsync(Guid id, bool isCorrect);
    }
}
