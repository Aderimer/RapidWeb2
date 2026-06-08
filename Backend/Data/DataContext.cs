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

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        // Model relationships and configurations can be defined here
        //}
    }
}