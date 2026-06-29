namespace WatchmanDevotional.DTOs
{
    public class DevotionalResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ScriptureReference { get; set; } = string.Empty;
        public string MemoryVerse { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? YoutubeVideoId { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
