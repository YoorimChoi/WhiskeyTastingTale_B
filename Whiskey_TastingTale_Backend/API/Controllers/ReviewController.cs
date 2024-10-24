using Microsoft.AspNetCore.Mvc;
using Whiskey_TastingTale_Backend.API.DTOs;
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

        [HttpGet("user")]
        public async Task<IActionResult> GetByUserId(int page =1)
        {
            var user_id = int.Parse(HttpContext.Items["UserId"].ToString());

            var result = await _repository.GetByUserId(user_id, page);
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            var result = await _repository.GetAll(page);
            return Ok(result);
        }

        [HttpGet("search/{serchOption}/{searchString}")]
        public async Task<IActionResult> GetByUserNickname(string serchOption, string searchString,  int page = 1)
        {
            var result = new ReviewUserWhiskeyPageDTO();
            if (serchOption.Equals("닉네임")) result = await _repository.GetByNickname(searchString, page);
            else if (serchOption.Equals("위스키명")) result = await _repository.GetByWhiskey(searchString, page);

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
