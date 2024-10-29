using Microsoft.AspNetCore.Mvc;
using Whiskey_TastingTale_Backend.Data.Repository;

namespace Whiskey_TastingTale_Backend.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly NotificationRepository _repository;

        public NotificationController(ILogger<NotificationController> logger, NotificationRepository repository)
        {
            _logger = logger;
            _repository = repository;   
        }

        [HttpGet("user/{user_id}")]
        public async Task<IActionResult> GetUserNotifications(int user_id, int page)
        {
            var notifications = await _repository.GetUserNotifications(user_id, page);
            return Ok(notifications);
        }

        [HttpPost("read/{notification_id}")]
        public async Task<IActionResult> MarkNotificationAsRead(int notification_id)
        {
            await _repository.MarkAsRead(notification_id);
            return Ok();
        }

        [HttpDelete("delete/{notification_id}")]
        public async Task<IActionResult> DeleteNotification(int notification_id)
        {
            await _repository.DeleteNotification(notification_id);
            return Ok();
        }

    }
}
