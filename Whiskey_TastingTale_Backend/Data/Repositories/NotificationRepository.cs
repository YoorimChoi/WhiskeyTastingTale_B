using Elastic.CommonSchema;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.API.DTOs;
using Whiskey_TastingTale_Backend.Data.Context;
using Whiskey_TastingTale_Backend.Data.Entities;
using Whiskey_TastingTale_Backend.Services;

namespace Whiskey_TastingTale_Backend.Data.Repository
{
    public class NotificationRepository
    {
        private readonly NotificationContext _notificationContext;
        private readonly UserContext _userContext; 
        private readonly NotificationService _notiService;

        public NotificationRepository(NotificationContext notificationContext, NotificationService notiService, UserContext userContext)
        {
            _notificationContext = notificationContext;
            _notiService = notiService;

            _userContext = userContext;
        }

        public async Task SendResultOfRequest(WhiskeyRequest request)
        {
            var notification = new Notification
            {
                user_id = request.user_id,
                message = $"위스키 추가 요청이 처리 되었습니다. : {request.name}, {(request.is_accepted ? "승낙":"거절")}",
                notification_type = "handleRequest",
                target_url = "/request_list",
                related_entity_id = null,
                is_read = false,
                created_at = DateTime.Now
            };

            var result = await _notificationContext.notifications.AddAsync(notification);
            await _notificationContext.SaveChangesAsync();

            _notiService.SendNotification(result.Entity, request.user_id);
        }

        public async Task AddWhiskeyNotification(int whiskeyId, string whiskeyName)
        {
            var userIDs = await _userContext.users.Where(u => u.role.Equals("user")).Select(x => x.user_id).ToListAsync();

            List<Notification> notifications = userIDs.Select(userID =>
            {
                return new Notification
                {
                    user_id = userID,
                    message = $"새로운 위스키가 추가되었습니다: {whiskeyName}",
                    notification_type = "addwhiskey",
                    target_url = "/details",
                    related_entity_id = whiskeyId,
                    is_read = false,
                    created_at = DateTime.Now
                };
            }).ToList();

            foreach (var notification in notifications)
            {
                var result =  await _notificationContext.notifications.AddAsync(notification);
                await _notificationContext.SaveChangesAsync();

                _notiService.SendNotification(result.Entity, result.Entity.user_id ?? 0);
            }

        }

        public async Task MarkAsRead(int notificationId)
        {
            var notification = await _notificationContext.notifications
                .FirstOrDefaultAsync(n => n.notification_id == notificationId);

            if (notification != null)
            {
                notification.is_read = true;
                await _notificationContext.SaveChangesAsync();
            }
        }

        public async Task DeleteNotification(int notificationId)
        {
            var notification = await _notificationContext.notifications
                .FirstOrDefaultAsync(n => n.notification_id == notificationId);

            if (notification != null)
            {
                _notificationContext.notifications.Remove(notification);
                await _notificationContext.SaveChangesAsync();
            }
        }

        internal async Task<NotificationPageDTO> GetUserNotifications(int user_id, int page = 1, int pageSize = 8)
        {
            var notifications = await _notificationContext.notifications.Where(n => n.user_id == user_id )
                .Skip((page-1)*pageSize).Take(pageSize).ToListAsync();

            var totalCount = await _notificationContext.notifications.Where(n => n.user_id == user_id).CountAsync();

            return new NotificationPageDTO()
            {
                notifications = notifications,
                totalCount = totalCount,
                page = page,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }
    }
}
