﻿using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Whiskey_TastingTale_Backend.Data.Entities;

namespace Whiskey_TastingTale_Backend.Data.Context
{
    public class WhiskeyContext : DbContext
    {
        internal DbSet<Whiskey> whiskeys { get; set; }

        public WhiskeyContext(DbContextOptions<WhiskeyContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Whiskey>()
                .ToTable(tb => tb.HasTrigger("UpdateCreated_Whiskeys"))
                .ToTable(tb => tb.HasTrigger("UpdateUpdated_Whiskeys"));

        }
    }
}
