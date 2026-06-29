namespace WatchmanDevotional.Models
{
    public class Devotionals
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ScriptureReference { get; set; } = string.Empty;
        public string MemoryVerse { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? YoutubeVideoId { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
