using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API_Exercise1_MovieCard.Data;
using API_Exercise1_MovieCard.Models.DTOs;
using API_Exercise1_MovieCard.Models.Entities;

namespace API_Exercise1_MovieCard.Controllers
{
    [Route("api/")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieCardContext _context;

        public MovieController(MovieCardContext context)
        {
            _context = context;
        }

        //GET: api/Movies
        [HttpGet("Movies")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var dto = _context.Movie.Select(m => new MovieDto(m.Id, m.Title, m.Rating, m.ReleaseDate, m.Description, m.Genres.Select(g => g.GenreName).ToList(), m.Actors.Select(a => a.Name).ToList(), m.Director.Name));
            return Ok(await dto.ToListAsync());
        }

        //GET: api/Movies/5
        [HttpGet("{id}", Name = "GetMovie")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var dto = await _context.Movie
                .Where(m => m.Id == id)
                .Select(m => new MovieByIdDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Rating = m.Rating,
                    ReleaseDate = m.ReleaseDate,
                    Description = m.Description,
                    Genres = m.Genres.Select(g => g.GenreName).ToList(),
                    Actors = m.Actors.Select(a => a.Name).ToList(),
                    Director = m.Director.Name
                })
                .FirstOrDefaultAsync();

            if (dto == null)
            {
                return NotFound();
            }

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<Movie>> CreateMovie(MovieForCreationDto newMovie)
        {
            if (newMovie == null)
            {
                return BadRequest(); 
            }

            var finalMovieToAdd = new Movie()
            {
                Title = newMovie.Title,
                Rating = newMovie.Rating,
                ReleaseDate = newMovie.ReleaseDate,
                Description = newMovie.Description,
                Genres = newMovie.Genres,
                Actors = newMovie.Actors,
                Director = newMovie.Director
            };

            _context.Movie.Add(finalMovieToAdd);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetMovie", finalMovieToAdd);
        }

        [HttpPut("{movieId}")]
        public async Task<ActionResult> UpdateMovie(int movieId, MovieForUpdateDto updateMovie)
        {
            var movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == movieId);
            if (movie == null)
            {
                return NotFound();
            }


            movie.Title = updateMovie.Title;
            movie.Rating = updateMovie.Rating;
            movie.ReleaseDate = updateMovie.ReleaseDate;
            movie.Description = updateMovie.Description;
            movie.Genres = updateMovie.Genres;
            movie.Actors = updateMovie.Actors;
            movie.Director = updateMovie.Director;

            return NoContent();

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
