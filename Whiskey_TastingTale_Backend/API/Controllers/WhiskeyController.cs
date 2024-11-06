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
        private readonly NotificationRepository _notificationRepository; 

        public WhiskeyController(ILogger<WhiskeyController> logger, WhiskeyRepository repository, NotificationRepository notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            _notificationRepository = notificationRepository;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _repository.GetAllWhiskey();
            return Ok(result);
        }

        [HttpGet("id/{whiskey_id}")]
        public async Task<IActionResult> GetByIDAsync(int whiskey_id)
        {
            var result = await _repository.GetById(whiskey_id);
            return Ok(result);
        }
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByNameAsync(string name, int page = 1)
        {
            var result = await _repository.GetByName(name, page);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> PostAsync(Whiskey whiskey)
        {
            var result = await _repository.AddWhiskey(whiskey);
            if (result != null)
            {
                await _notificationRepository.AddWhiskeyNotification(result.whiskey_id, result.whiskey_name);
                return Ok(result);
            }
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
        [HttpDelete("{whiskey_id}")]
        public async Task<IActionResult> DeleteAsync(int whiskey_id)
        {
            var result = await _repository.DeleteWhiskey(whiskey_id);
            if (result != null) return Ok(result);
            else return BadRequest(whiskey_id);
        }
    }
}
