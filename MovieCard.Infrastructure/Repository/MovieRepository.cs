using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieCard.Contracts;
using MovieCard.Infrastructure.Data;
using MovieCard.Models.Entities;
using MovieCard.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MovieCard.Infrastructure.Repository
{
    public class MovieRepository : RepositoryBase<Movie>, IMovieRepository
    {
        private readonly IMapper _mapper;
        public MovieRepository(MovieCardContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        //OBJECT MARKERAD - Titta in på att skapa en basklass/interface för MovieDto och MovieDetailDto
        public async Task<IEnumerable<object>> GetMoviesAsync(string? title, string? genre, string? director, string? actor, string? releaseDate, string? sortBy, string? sortOrder, bool trackChanges, bool detailed, IMapper mapper)
        {
            var query = GetAll(trackChanges);

            if (!string.IsNullOrEmpty(title))
            {
                title = title.Trim();
                query = query.Where(m => m.Title == title);
            }
            if (!string.IsNullOrEmpty(genre))
            {
                genre = genre.Trim();
                query = query.Include(m => m.Genres).Where(m => m.Genres.Any(g => g.GenreName == genre));
            }

            if (!string.IsNullOrEmpty(director))
            {
                director = director.Trim().ToLower();
                query = query.Where(m => m.Director.Name.Replace(" ", "").ToLower().Equals(director));
            }

            if (!string.IsNullOrEmpty(actor))
            {
                actor = actor.Trim().ToLower();
                query = query.Include(m => m.Actors).Where(m => m.Actors.Any(a => a.Name.Replace(" ", "").ToLower().Equals(actor)));
            }

            if (!string.IsNullOrEmpty(releaseDate))
            {
                releaseDate = releaseDate.Trim();
                query = query.Where(m => m.ReleaseDate == releaseDate);
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                bool isDescending = !string.IsNullOrEmpty(sortOrder) && sortOrder.Equals("desc");

                query = sortBy.ToLower() switch
                {
                    "title" => isDescending ? query.OrderByDescending(m => m.Title) : query.OrderBy(m => m.Title),
                    "rating" => isDescending ? query.OrderByDescending(m => m.Rating) : query.OrderBy(m => m.Rating),
                    "realeasedate" => isDescending ? query.OrderByDescending(m => m.ReleaseDate) : query.OrderBy(m => m.ReleaseDate),
                    _ => query.OrderBy(m => m.Id)
                };
            }

            if (detailed)
            {
                var queryResults = await query
                .Include(m => m.Director)
                .Include(m => m.Director.ContactInfo)
                .Include(m => m.Actors)
                .Include(m => m.Genres)
                .ProjectTo<MovieDetailDto>(mapper.ConfigurationProvider)
                .ToListAsync();

                return queryResults;
            }
            else
            {
                var queryResults = await query
                    .Include(m => m.Director)
                    .ProjectTo<MovieDto>(mapper.ConfigurationProvider)
                    .ToListAsync();

                return queryResults;
            }
        }

        public async Task<Movie?> GetMovieByIdAsync(int id, bool trackChanges)
        {
            return await FindByCondition(m => m.Id.Equals(id), trackChanges).Include(m => m.Director).FirstOrDefaultAsync();
        }

        public async Task<Movie?> GetMovieByTitleAsync(string title, bool trackChanges)
        {
            return await FindByCondition(m => m.Title.Equals(title), trackChanges).Include(m => m.Director).FirstOrDefaultAsync();
        }

        public async Task<Movie> CreateNewMovieAsync(MovieForCreationDto newMovie)
        {
            var movieExists = await FindByCondition(m => m.Title.Equals(newMovie.Title), false).FirstOrDefaultAsync();
            if (movieExists != null)
            {
                return movieExists;
            }

            var finalMovieToAdd = _mapper.Map<Movie>(newMovie);
            await CreateAsync(finalMovieToAdd);
            await Context.SaveChangesAsync();
            return finalMovieToAdd;
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movieToBeDeleted = await FindByCondition(m => m.Id.Equals(id), false).FirstOrDefaultAsync();
            if (movieToBeDeleted == null)
            {
                return false; 
            }
            Delete(movieToBeDeleted);
            await Context.SaveChangesAsync();
            return true;
        }
    }
}