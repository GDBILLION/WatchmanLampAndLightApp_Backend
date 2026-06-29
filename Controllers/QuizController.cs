using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatchmanDevotional.DTOs;
using WatchmanDevotional.Interfaces;
using WatchmanDevotional.Models;

namespace WatchmanDevotional.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizRepository _repository;
        public QuizController(IQuizRepository repository)
        {
            _repository = repository;
        }
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitAnswer([FromBody] QuizSubmissionDto dto)
        {
            var alreadySubmitted = await _repository.HasAlreadySubmittedTodayAsync(dto.PhoneNumber);

            if (alreadySubmitted)
            {
                return BadRequest(dto.FullName + ", you have already submitted an answer");
            }

            var entry = new QuizEntry
            {
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                Answer = dto.Answer,
                SubmittedAt = DateTime.UtcNow,
                IsCorrect = false
            };

            await _repository.AddEntryAsync(entry);

            return Ok(new { message = "Submission successful! Watch out for the winners' announcement" });
        }

        [HttpGet("winners")]
        public async Task<IActionResult> GetWinners()
        {
            var winners = await _repository.GetRandomWinnersAsync(5);
            return Ok(winners);
        }

        [HttpGet("admin/today-submissions")]
        public async Task<IActionResult> GetTodaySubmissions()
        {
            var entries = await _repository.GetTodayAllEntriesAsync();
            return Ok(entries);
        }

        // 🌟 NEW FOR PHASE 1: TOGGLE GRADE STATUS FROM FRONTEND CHECKBOX
        [HttpPut("admin/grade/{id}")]
        public async Task<IActionResult> GradeSubmission(Guid id, [FromBody] bool isCorrect)
        {
            var success = await _repository.UpdateEntryGradeAsync(id, isCorrect);
            if (!success)
            {
                return NotFound(new { message = "Submission entry not found." });
            }

            return Ok(new { message = "Submission graded successfully!" });
        }
    }
}
