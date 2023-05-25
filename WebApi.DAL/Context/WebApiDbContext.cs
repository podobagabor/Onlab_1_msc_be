using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.DAL.Entities;

namespace WebApi.DAL.Context
{
    public class WebApiDbContext : IdentityDbContext<User, IdentityRole<int> ,int>
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<IngredientGroup> IngredientGroups { get; set; }
        public DbSet<Description> Descriptions { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientItem> IngredientItems { get; set; }
        public override DbSet<User> Users { get; set; }
        public WebApiDbContext(DbContextOptions<WebApiDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
           .Property(p => p.Id)
           .ValueGeneratedOnAdd()
           .HasConversion<long>();
        }
    }
}
