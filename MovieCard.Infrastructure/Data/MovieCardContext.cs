using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieCard.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MovieCard.Infrastructure.Data
{
    public class MovieCardContext : DbContext
    {
        public MovieCardContext (DbContextOptions<MovieCardContext> options)
            : base(options)
        {
        }

        public DbSet<Actor> Actor => Set<Actor>();
        public DbSet<Director> Director => Set<Director>();
        public DbSet<Genre> Genre => Set<Genre>();
        public DbSet<Movie> Movie => Set<Movie>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Director)
                .WithMany(d => d.Movies)
                .HasForeignKey(m => m.DirectorId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //ChangeTracker.DetectChanges();

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
