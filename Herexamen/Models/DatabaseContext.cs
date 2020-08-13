using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Herexamen.Models;

namespace Herexamen.Models
{
    public class DatabaseContext : DbContext

    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<List> List { get; set; }
        public DbSet<Vote> Vote { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<User>().HasMany(u => u.Lists).WithOne(u => u.User).IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany(v => v.Votes).WithOne(u => u.User).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Item>().HasOne(l => l.List).WithMany(i => i.Items).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Item>().HasMany(v => v.Votes).WithOne(i => i.Item).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<List>().HasOne(u => u.User).WithMany(l => l.Lists).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<List>().HasMany(i => i.Items).WithOne(l => l.List).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<List>().HasOne(r => r.Category).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Vote>().HasOne(u => u.User).WithMany(v => v.Votes).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Vote>().HasOne(i => i.Item).WithMany(v => v.Votes).OnDelete(DeleteBehavior.Restrict);
            
         // modelBuilder.Entity<Item>().ToTable("Item");
          //  modelBuilder.Entity<List>().ToTable("List");
          //  modelBuilder.Entity<Vote>().ToTable("Vote");
        }

        public DbSet<Herexamen.Models.Category> Category { get; set; }

       
    }
}
