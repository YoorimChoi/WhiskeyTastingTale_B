using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.Model;

namespace Whiskey_TastingTale_Backend.Repository
{
    public class UserContext : DbContext
    {
        internal DbSet<User> users { get; set; }
        public UserContext(DbContextOptions options) : base(options)
        {

        }

    }
}