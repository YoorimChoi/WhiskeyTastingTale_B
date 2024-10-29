using Elastic.CommonSchema;
using Microsoft.AspNetCore.SignalR;
using Whiskey_TastingTale_Backend.Data.Entities;

namespace Whiskey_TastingTale_Backend.Services
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var user_id = Context.GetHttpContext().Request.Headers["user_id"].ToString();
            await Groups.AddToGroupAsync(Context.ConnectionId, user_id);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user_id = Context.GetHttpContext().Request.Headers["user_id"].ToString();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, user_id);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
