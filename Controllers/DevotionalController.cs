using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatchmanDevotional.Models;
using WatchmanDevotional.Services;

namespace WatchmanDevotional.Controllers
{
    [Route("api/devotionals")]
    [ApiController]
    public class DevotionalController : ControllerBase
    {
        private readonly DevotionalService _service;
        public DevotionalController(DevotionalService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Add logic here to return all devotionals
            var result = await _service.GetAllDevotionalsAsync();
            return Ok(result);
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetToday()
        {
            var result = await _service.GetTodayDevotionalAsync();
            if (result == null) return NotFound("No devotional found for today");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDevotional([FromBody] Devotionals devotional)
        {
            if (devotional == null) return BadRequest("Invalid devotional data.");

            var result = await _service.CreateDevotinalAsync(devotional);

            return CreatedAtAction(nameof(GetToday), result);
        }

        [HttpGet("archive")]
        public async Task<IActionResult> GetArchive()
        {
            var result = await _service.GetArchiveAsync();
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            var result = await _service.SearchDevotionalAsync(q);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="SuperAdmin")]
        public async Task<IActionResult> DeleteDevotional(Guid id)
        {
            var deleted = await _service.DeleteDevotionalAsync(id);
            if (!deleted) return NotFound("Devotional not found.");

            return NoContent(); 
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> UpdateDevotional(Guid id, [FromBody] Devotionals updatedData)
        {
            if (updatedData == null) return BadRequest("Invalid devotional data.");

            // Ensure we are modifying the target entry
            updatedData.Id = id;
            updatedData.PublishDate = DateTime.SpecifyKind(updatedData.PublishDate.Date, DateTimeKind.Utc);

            // We will build UpdateAsync in the next file below
            var success = await _service.UpdateDevotionalAsync(id, updatedData);
            if (!success) return NotFound("Devotional entry not found to modify.");

            return Ok(updatedData);
        }
    }
}
