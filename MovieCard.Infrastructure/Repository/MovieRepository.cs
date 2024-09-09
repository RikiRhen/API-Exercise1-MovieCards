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
    public class MovieRepository : RepositoryBase<Movie>, IMovieRepository
    {

        public MovieRepository(MovieCardContext context): base(context){}

        public async Task<IEnumerable<Movie>> GetMoviesAsync(bool trackChanges)
        {
            return await GetAll(trackChanges).ToListAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(int id, bool trackChanges)
        {
            return await FindByCondition(m => m.Id.Equals(id), trackChanges).FirstOrDefaultAsync();
        }
    }
}
