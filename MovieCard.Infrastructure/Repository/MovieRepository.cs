using Microsoft.EntityFrameworkCore;
using MovieCard.Contracts;
using MovieCard.Infrastructure.Data;
using MovieCard.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCard.Infrastructure.Repository
{
    internal class MovieRepository : IMovieRepository
    {
        private readonly MovieCardContext _context;

        public MovieRepository(MovieCardContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            return await _context.Movie.ToListAsync();
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            return await _context.Movie.FindAsync(id);
        }
    }
}
