using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.Data.Entities;

namespace Whiskey_TastingTale_Backend.Data.Context
{
    public class UserContext : DbContext
    {
        internal DbSet<User> users { get; set; }
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

    }
}