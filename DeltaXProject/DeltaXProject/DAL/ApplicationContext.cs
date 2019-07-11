using DeltaXProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace DeltaXProject.DAL
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("DefaultConnection")
        {
        }

        public DbSet<Movie> Movies { get; set; }
        
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Actor> Actors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Movie>()
             .HasMany(c => c.Actors).WithMany(i => i.Movies)
             .Map(t => t.MapLeftKey("MovieID")
             .MapRightKey("ActorID")
             .ToTable("MovieActor"));
        }

    }
}