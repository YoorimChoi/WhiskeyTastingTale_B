using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using Whiskey_TastingTale_Backend.Model;
using Whiskey_TastingTale_Backend.Repository;

namespace Whiskey_TastingTale_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserRepository _repository;

        public UserController(ILogger<UserController> logger, UserRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _repository.GetAllAsync(); 
            return Ok(result); 
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetAsync(string email)
        {
            var result = await _repository.GetByEmailAsync(email);
            if(result != null) return Ok(result);
            else return BadRequest(email);
        }

        [HttpGet("salt/{email}")]
        public async Task<IActionResult> GetSaltAsync(string email)
        {
            var result = await _repository.GetSaltAsync(email);
            if (result != null) return Ok(result); 
            else return BadRequest(email);
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync(User user)
        {
            var result = await _repository.AddUserAsync(user);
            if (result != null) return Ok(result); 
            else return BadRequest(user);  
        }

        [HttpPost("login")]
        public async Task<IActionResult> PostAsync(string email, string password)
        {
            var result = await _repository.LoginAsync(email, password);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(User user)
        {
            var result = await _repository.UpdateUserAsync(user);
            if(result != null) return Ok(result);
            else return BadRequest(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _repository.DeleteUserAsync(id);
            if (result != null) return Ok(result);
            else return BadRequest(id);
        }
    }
}
