using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.Model;

namespace Whiskey_TastingTale_Backend.Repository.Context
{
    public class WhiskeyContext : DbContext
    {
        internal DbSet<Whiskey> whiskeys { get; set; }

        public WhiskeyContext(DbContextOptions options) : base(options)
        {

        }

    }
}
