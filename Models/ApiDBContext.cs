using dockerapi.Maps;
using dockerapi.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace dockerapi.Models
{
#pragma warning disable CS1591
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BlogMap());
        }
    }
#pragma warning restore CS1591
}