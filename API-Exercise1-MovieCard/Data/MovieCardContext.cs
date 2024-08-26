using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API_Exercise1_MovieCard.Models.Entities;
using API_Exercise1_MovieCard.Models.DTOs;

namespace API_Exercise1_MovieCard.Data
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

        //public DbSet<API_Exercise1_MovieCard.Models.Entities.Movie> Movie { get; set; } = default!;
        //public DbSet<API_Exercise1_MovieCard.Models.DTOs.MovieDto> MovieDto { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Director)
                .WithMany(d => d.Movies)
                .HasForeignKey(m => m.DirectorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
