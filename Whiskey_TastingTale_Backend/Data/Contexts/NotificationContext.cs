using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.Data.Entities;

namespace Whiskey_TastingTale_Backend.Data.Context
{
    public class NotificationContext :DbContext
    {
        internal DbSet<Notification> notifications { get; set; }
        public NotificationContext(DbContextOptions<NotificationContext> options) : base(options)
        {

        }
    }
}
