using Elastic.CommonSchema;
using Microsoft.AspNetCore.SignalR;
using Whiskey_TastingTale_Backend.Data.Entities;

namespace Whiskey_TastingTale_Backend.Services
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotificationToAll(Notification notification)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
        }

        public async Task SendNotification(Notification notification, int user_id)
        {
            await _hubContext.Clients.Group(user_id.ToString()).SendAsync("ReceiveNotification", notification);
        }
    }

}
