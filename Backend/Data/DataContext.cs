using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        // DbSet for each model
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Merch> Merch { get; set; }
        public DbSet<Category> Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        // Model relationships and configurations can be defined here
            modelBuilder.Entity<Merch>()
            .HasOne(c => c.Category)
            .WithMany(m => m.Merch)
            .HasForeignKey(m => m.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}