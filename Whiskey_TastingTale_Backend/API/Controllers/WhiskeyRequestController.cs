using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Whiskey_TastingTale_Backend.Data.Entities;
using Whiskey_TastingTale_Backend.Data.Repository;

namespace Whiskey_TastingTale_Backend.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class WhiskeyRequestController : ControllerBase
    {
        private readonly ILogger<WhiskeyRequestController> _logger;
        private readonly WhiskeyRequestRepository _repository;
        public WhiskeyRequestController(ILogger<WhiskeyRequestController> logger, WhiskeyRequestRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _repository.GetAllRequest();
            return Ok(result);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetAsync()
        {
            var user_id = int.Parse(HttpContext.Items["UserId"].ToString());

            var result = await _repository.GetByUserId(user_id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(WhiskeyRequest request)
        {
            var result = await _repository.AddWhiskeyRequest(request);
            if (result != null) return Ok(result);
            else return BadRequest(request);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(WhiskeyRequest request)
        {
            var user_id = int.Parse(HttpContext.Items["UserId"].ToString());
            var role = HttpContext.Items["UserRole"].ToString();
            if (role.Equals("user") && request.user_id != user_id) return BadRequest(request);


            var result = await _repository.UpdateWhiskeyReqeust(request);
            return Ok(result);
        }

        [HttpDelete("{request_id}")]
        public async Task<IActionResult> DeleteAsync(int request_id)
        {
            var user_id = int.Parse(HttpContext.Items["UserId"].ToString());
            var role = HttpContext.Items["UserRole"].ToString();

            var result = await _repository.DeleteWhiskeyReqeust(request_id, user_id, role);
            if (result != null) return Ok(result); 
            else return BadRequest(request_id);
        }
    }
}
