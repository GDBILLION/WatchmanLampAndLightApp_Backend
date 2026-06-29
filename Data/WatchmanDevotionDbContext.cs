using Microsoft.EntityFrameworkCore;
using WatchmanDevotional.Models;

namespace WatchmanDevotional.Data
{
    public class WatchmanDevotionDbContext : DbContext
    {
        public WatchmanDevotionDbContext(DbContextOptions<WatchmanDevotionDbContext> options)
        : base(options) { }

        public DbSet<Devotionals> Devotionals => Set<Devotionals>();
        public DbSet<QuizEntry> QuizEntries => Set<QuizEntry>();

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Industry Standard: Ensure PhoneNumbers are indexed for fast lookups
            // and to help prevent duplicate entries during traffic spikes.
            modelBuilder.Entity<QuizEntry>()
                .HasIndex(q => new { q.PhoneNumber, q.SubmittedAt })
                .IsUnique();
        }
    }
}

