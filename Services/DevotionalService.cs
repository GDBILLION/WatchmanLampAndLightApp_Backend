using WatchmanDevotional.DTOs;
using WatchmanDevotional.Interfaces;
using WatchmanDevotional.Models;

namespace WatchmanDevotional.Services
{
    public class DevotionalService
    {
        private readonly IDevotionalRepository _repository;
        public DevotionalService(IDevotionalRepository repository)
        {
            _repository = repository;
        }
        public async Task<DevotionalResponseDto?> GetTodayDevotionalAsync()
        {
            var lagosTime = DateTime.UtcNow.AddHours(1);
            var devotionals = await _repository.GetByDateAsync(lagosTime.Date);

            if (devotionals == null) return null;

            return MapToDto(devotionals);
        }
        public async Task<IEnumerable<DevotionalResponseDto>> GetArchiveAsync()
        {
            var devotionals = await _repository.GetAllPastAsync();
            return devotionals.Select(MapToDto);
        }

        public async Task<IEnumerable<DevotionalResponseDto>> GetAllDevotionalsAsync()
        {
            var devotionals = await _repository.GetAllAsync(); // Assuming your repo has an GetAllAsync()
            return devotionals.Select(MapToDto);
        }

        public async Task<IEnumerable<DevotionalResponseDto>>SearchDevotionalAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return Enumerable.Empty<DevotionalResponseDto>();

            var devotionals = await _repository.SearchAsync(term);
            return devotionals.Select(MapToDto);
        }

        public async Task<DevotionalResponseDto>CreateDevotinalAsync(Devotionals devotional)
        {
            devotional.Id = Guid.NewGuid();
            devotional.PublishDate = DateTime.SpecifyKind(devotional.PublishDate.Date, DateTimeKind.Utc);
            devotional.CreatedAt = DateTime.UtcNow;

            await _repository.AddAsync(devotional);
            return MapToDto(devotional);
        }

        private DevotionalResponseDto MapToDto(Devotionals d)
        {
            return new DevotionalResponseDto
            {
                Id = d.Id,
                Title = d.Title,
                Content = d.Content,
                MemoryVerse = d.MemoryVerse,
                ScriptureReference = d.ScriptureReference,
                YoutubeVideoId = d.YoutubeVideoId,
                PublishDate = d.PublishDate
            };
        }

        public async Task<bool> DeleteDevotionalAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> UpdateDevotionalAsync(Guid id, Devotionals updatedData)
        {
            // Pass execution down to the repository layer
            return await _repository.UpdateAsync(id, updatedData);
        }

    }
}
