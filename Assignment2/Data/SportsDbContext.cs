using Assignment2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Assignment2.Data
{
    public class SportsDbContext : DbContext
    {
        public SportsDbContext(DbContextOptions<SportsDbContext> options) : base(options) { }

        public DbSet<Fan> Fans { get; set; }
        public DbSet<SportClub> SportClubs { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<News> News { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscription>()
                .HasKey(s => new { s.FanId, s.SportClubId });
            modelBuilder.Entity<Subscription>()
                .HasOne(f => f.Fan)
                .WithMany(s => s.Subscriptions)
                .HasForeignKey(sub => sub.FanId);
            modelBuilder.Entity<Subscription>()
                .HasOne(sc => sc.SportClub)
                .WithMany(s => s.Subscriptions)
                .HasForeignKey(sub => sub.SportClubId);

            modelBuilder.Entity<News>()
                .HasKey(n => new { n.Id });
            modelBuilder.Entity<News>()
                .HasOne(s => s.SportClub)
                .WithMany(n => n.News)
                .HasForeignKey(news => news.SportClubId);

            modelBuilder.Entity<Fan>().ToTable("Fan");
            modelBuilder.Entity<SportClub>().ToTable("SportClub");
            modelBuilder.Entity<Subscription>().ToTable("Subscription");
            modelBuilder.Entity<News>().ToTable("News");

        }

    }
}