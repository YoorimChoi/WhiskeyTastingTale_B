using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using Whiskey_TastingTale_Backend.Model;
using Whiskey_TastingTale_Backend.Repository;

namespace Whiskey_TastingTale_Backend.Controllers
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

        [HttpPost]
        public async Task<IActionResult> PostAsync(Whiskey whiskey)
        {
            var result = await _repository.AddWhiskey(whiskey);
            return Ok(result);
        }

        [HttpPut()]
        public async Task<IActionResult> PutAsync(Whiskey whiskey)
        {
            var result = await _repository.UpdateWhiskey(whiskey);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _repository.DeleteWhiskey(id);
            return Ok(result);
        }
    }
}
