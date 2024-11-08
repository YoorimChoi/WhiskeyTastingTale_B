using Microsoft.AspNetCore.Mvc;
using Whiskey_TastingTale_Backend.Data.Entities;
using Whiskey_TastingTale_Backend.Data.Repository;

namespace Whiskey_TastingTale_Backend.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WishController : ControllerBase
    {
        private readonly ILogger<WishController> _logger;
        private readonly WishRepository _repository;

        public WishController(ILogger<WishController> logger, WishRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("whiskey/{whiskey_id}")]
        public async Task<IActionResult> Get(int whiskey_id)
        {
            var user_id = int.Parse(HttpContext.Items["UserId"].ToString());

            var result = await _repository.Get(user_id, whiskey_id);
            return Ok(result);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetByUserId(int page=1)
        {
            var user_id = int.Parse(HttpContext.Items["UserId"].ToString());

            var result = await _repository.GetByUserId(user_id, page);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Wish wish)
        {
            var result = await _repository.Create(wish);
            if (result != null) return Ok(result);
            else return BadRequest();
        }

        [HttpDelete("{wish_id}")]
        public async Task<IActionResult> Delete(int wish_id)
        {
            var result = await _repository.Delete(wish_id);
            if (result != null) return Ok(result);
            else return BadRequest();
        }
    }
}
