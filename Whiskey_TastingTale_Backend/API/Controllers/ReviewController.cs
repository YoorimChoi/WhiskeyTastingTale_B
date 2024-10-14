using Microsoft.AspNetCore.Mvc;
using Whiskey_TastingTale_Backend.Data.Entities;
using Whiskey_TastingTale_Backend.Data.Repository;

namespace Whiskey_TastingTale_Backend.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly ReviewRepository _repository;

        public ReviewController(ILogger<ReviewController> logger, ReviewRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("whiskey/{whiskey_id}")]
        public async Task<IActionResult> GetByWhiskeyId(int whiskey_id, int page = 1)
        {
            var result = await _repository.GetByWhiskeyId(whiskey_id, page);
            return Ok(result);
        }

        [HttpGet("user/{user_id}")]
        public async Task<IActionResult> GetByUserId(int user_id, int page =1)
        {
            var result = await _repository.GetByUserId(user_id, page);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Review review)
        {
            var result = await _repository.Create(review);
            if (result != null) return Ok(result);
            else return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Put(Review review)
        {
            var result = await _repository.Update(review);
            if (result != null) return Ok(result);
            else return BadRequest();
        }

        [HttpDelete("{review_id}")]
        public async Task<IActionResult> Delete(int review_id)
        {
            var result = await _repository.Delete(review_id);
            if (result != null) return Ok(result);
            else return BadRequest();
        }
    }
}
