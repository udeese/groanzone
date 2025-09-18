using Microsoft.EntityFrameworkCore;
using GroanZone.Models;

namespace GroanZone.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Joke> Jokes => Set<Joke>();
        public DbSet<Rating> Ratings => Set<Rating>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<Joke>()
                .HasOne(j => j.Author)
                .WithMany(u => u.Jokes)
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Joke)
                .WithMany(j => j.Ratings)
                .HasForeignKey(r => r.JokeId)
                .OnDelete(DeleteBehavior.Cascade);

            // one rating per user
            modelBuilder.Entity<Rating>()
                .HasIndex(r => new { r.UserId, r.JokeId })
                .IsUnique();
        }
    }
}