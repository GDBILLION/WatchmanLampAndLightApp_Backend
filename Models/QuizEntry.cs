namespace WatchmanDevotional.Models
{
    public class QuizEntry
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public bool IsCorrect { get; set; } = false;
        public bool WasSelelectedAsWinner { get; set; } = false;
        public DateTime? WinningDate { get; set; }
    }
}
