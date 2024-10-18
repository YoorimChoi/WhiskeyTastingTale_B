using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Whiskey_TastingTale_Backend.Data.Entities;

namespace Whiskey_TastingTale_Backend.Data.Context
{
    public class WhiskeyRequestContext : DbContext
    {
        internal DbSet<WhiskeyRequest> whiskeyRequests { get; set; }

        public WhiskeyRequestContext(DbContextOptions<WhiskeyRequestContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder
            //    .Entity<Whiskey>()
            //    .ToTable(tb => tb.HasTrigger("UpdateCreated_Whiskeys"))
            //    .ToTable(tb => tb.HasTrigger("UpdateUpdated_Whiskeys"));

        }
    }
}
