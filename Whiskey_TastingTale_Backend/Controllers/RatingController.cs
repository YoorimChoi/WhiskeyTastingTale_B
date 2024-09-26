using Microsoft.AspNetCore.Mvc;
using Whiskey_TastingTale_Backend.Repository;

namespace Whiskey_TastingTale_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly ILogger<RatingController> _logger;
        private readonly RatingRepository _repository; 

        public RatingController(ILogger<RatingController> logger, RatingRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("{whiskey_id}")]
        public async Task<IActionResult> Get(int whiskey_id)
        {
            var result = await _repository.Get(whiskey_id);
            return Ok(result);
        }
    }
}
