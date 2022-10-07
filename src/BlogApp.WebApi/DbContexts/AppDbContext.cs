using BlogApp.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.WebApi.DbContexts
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; } = null!;

        public virtual DbSet<BlogPost> BlogPosts { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        }
    }
}
