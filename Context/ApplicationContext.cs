using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaserCraftHub.Models;
using Microsoft.EntityFrameworkCore;

namespace LaserCraftHub.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Craft> Crafts { get; set; }
        public DbSet<Like> Likes { get; set; }
        // public DbSet<Message> Messages { get; set; }
        // public DbSet<Comment> Comments { get; set; }
    }
}