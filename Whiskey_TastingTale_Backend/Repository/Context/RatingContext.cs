using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.Model;

namespace Whiskey_TastingTale_Backend.Repository.Context
{
    public class RatingContext : DbContext
    {
        internal DbSet<Rating> ratings { get; set; }

        public RatingContext(DbContextOptions<RatingContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Rating>()
                .ToTable(tb => tb.HasTrigger("UpdateCreatedDate_Ratings"))
                .ToTable(tb => tb.HasTrigger("UpdateUpdatedDate_Ratings"));
        }
    }
}
