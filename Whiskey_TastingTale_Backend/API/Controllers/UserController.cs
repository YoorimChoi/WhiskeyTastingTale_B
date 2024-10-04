using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Whiskey_TastingTale_Backend.API.DTOs;
using Whiskey_TastingTale_Backend.Data.Entities;
using Whiskey_TastingTale_Backend.Data.Repository;

namespace Whiskey_TastingTale_Backend.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserRepository _repository;
        private readonly IConfiguration _configuration;

        public UserController(ILogger<UserController> logger, UserRepository repository, IConfiguration configuration)
        {
            _logger = logger;
            _repository = repository;
            _configuration = configuration;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _repository.GetAllAsync();
            return Ok(result);
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
        public async Task<IActionResult> LoginAsync(User user)
        {
            var result = await _repository.LoginAsync(user.email, user.password_hash);
            if (result != null)
            {
                var token = GenerateJwtToken(user.email, result);
                return Ok(new LoginDTO(){ 
                    token = token, 
                    email = result.email ?? "unknown",
                    user_id = result.user_id, 
                    nickname = result.nickname ?? "unknown",
                    role = result.role ?? "unknown"
                });
            }
            return Unauthorized();
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(User user)
        {
            //TODO : user의 role 에 따라서 동작이 달라져야함 -> user = 자기 것만 변경 가능, admin = 모두 변경가능 
            var result = await _repository.UpdateUserAsync(user);
            if (result != null) return Ok(result);
            else return BadRequest(user);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _repository.DeleteUserAsync(id);
            if (result != null) return Ok(result);
            else return BadRequest(id);
        }

        private string GenerateJwtToken(string email, User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.nickname),
                new Claim(ClaimTypes.Email, user.email),
                new Claim(ClaimTypes.Role, user.role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
