using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Whiskey_TastingTale_Backend.Data.Entities;
using Whiskey_TastingTale_Backend.Data.Repository;

namespace Whiskey_TastingTale_Backend.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WhiskeyController : ControllerBase
    {
        private readonly ILogger<WhiskeyController> _logger;
        private readonly WhiskeyRepository _repository;

        public WhiskeyController(ILogger<WhiskeyController> logger, WhiskeyRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _repository.GetAllWhiskey();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            var result = await _repository.GetById(id);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> PostAsync(Whiskey whiskey)
        {
            var result = await _repository.AddWhiskey(whiskey);
            if (result != null) return Ok(result);
            else return BadRequest(whiskey);
        }

        [Authorize(Roles = "admin")]
        [HttpPut()]
        public async Task<IActionResult> PutAsync(Whiskey whiskey)
        {
            var result = await _repository.UpdateWhiskey(whiskey);
            if (result != null) return Ok(result);
            else return BadRequest(whiskey);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _repository.DeleteWhiskey(id);
            if (result != null) return Ok(result);
            else return BadRequest(id);
        }
    }
}
