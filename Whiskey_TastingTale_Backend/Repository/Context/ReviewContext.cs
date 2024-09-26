using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.Model;

namespace Whiskey_TastingTale_Backend.Repository.Context
{
    public class ReviewContext : DbContext
    {
        internal DbSet<Review> reviews { get; set; }

        public ReviewContext(DbContextOptions<ReviewContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Review>()
                .ToTable(tb => tb.HasTrigger("UpdateCreatedDate_Reviews"))
                .ToTable(tb => tb.HasTrigger("UpdateUpdatedDate_Reviews"))
                .ToTable(tb => tb.HasTrigger("UpdateTotalRating_Reviews"));

        }
    }
}
