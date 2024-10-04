using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.Data.Entities;

namespace Whiskey_TastingTale_Backend.Data.Context
{
    public class WishContext : DbContext
    {
        internal DbSet<Wish> wishs { get; set; }

        public WishContext(DbContextOptions<WishContext> options) : base(options)
        {

        }
    }
}
