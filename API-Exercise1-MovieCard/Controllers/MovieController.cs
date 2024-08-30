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
    [Route("api")]
    [ApiController]
    [Produces("application/json")]
    public class MovieController : ControllerBase
    {
        private readonly MovieCardContext _context;

        public MovieController(MovieCardContext context)
        {
            _context = context;
        }
        
        // GET MOVIES
        //GET: api/Movies
        [HttpGet("Movies")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var dto = _context.Movie.Select(m => new MovieDto(m.Id, m.Title, m.Rating, m.ReleaseDate, m.Description, m.Director.Name));
            return Ok(await dto.ToListAsync());
        }

        // GET ACTORS
        //GET: api/Actors
        [HttpGet("Actors")]
        public async Task<ActionResult<IEnumerable<ActorDto>>> GetActors()
        {
            var dto = _context.Actor.Select(a => new ActorDto(a.Name, a.DateOfBirth));
            return Ok(await dto.ToListAsync());
        }

        //GET DIRECTORS
        //GET: api/Directors
        [HttpGet("Directors")]
        public async Task<ActionResult<IEnumerable<DirectorDto>>> GetDirectors()
        {
            var dto = _context.Director.Include(d => d.ContactInfo).Select(d => new DirectorDto(d.Name, d.DateOfBirth, new ContactInfoDto(d.ContactInfo.Email, d.ContactInfo.PhoneNr)));
            return Ok(await dto.ToListAsync());
        }

        //GET MOVIE BY ID
        //GET: api/Movies/1
        [HttpGet("Movies/{id}", Name = "GetMovie")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var dto = await _context.Movie
                .Where(m => m.Id == id)
                .Select(m => new MovieDto
                (
                    m.Id,
                    m.Title,
                    m.Rating,
                    m.ReleaseDate,
                    m.Description,
                    m.Director.Name
                ))
                .FirstOrDefaultAsync();

            if (dto == null)
            {
                return NotFound();
            }

            return Ok(dto);
        }

        //GET ACTOR BY ID
        //GET: api/Actors/1
        [HttpGet("Actors/{id}", Name = "GetActor")]
        public async Task<ActionResult<ActorDto>> GetActor(int id)
        {
            var dto = await _context.Actor
                .Where(a => a.Id == id)
                .Select(a => new ActorDto
                    (
                        a.Name,
                        a.DateOfBirth
                    ))
                .FirstOrDefaultAsync();

            if (dto == null)
            {
                return NotFound();
            }

            return Ok(dto);
        }

        //GET DIRECTOR BY ID
        //GET: api/Directors/1
        [HttpGet("Directors/{id}", Name = "GetDirector")]
        public async Task<ActionResult<DirectorDto>> GetDirector(int id)
        {
            var dto = await _context.Director
                .Include(c => c.ContactInfo)
                .Where(d => d.Id == id)
                .Select(d => new DirectorDto
                    (
                        d.Name,
                        d.DateOfBirth,
                        new ContactInfoDto(d.ContactInfo.Email, d.ContactInfo.PhoneNr)
                    ))
                .FirstOrDefaultAsync();

            if (dto == null)
            {
                return NotFound();
            }

            return Ok(dto);
        }

        [HttpPost("Movies")]
        public async Task<ActionResult<Movie>> CreateMovie(MovieForCreationDto newMovie)
        {
            if (newMovie == null)
            {
                return BadRequest("newMovie was Null");
            }

            var director = await _context.Director.FindAsync(newMovie.DirectorId);
            if (director == null)
            {
                return NotFound($"Director with ID {newMovie.DirectorId} was not found");
            }

            var finalMovieToAdd = new Movie()
            {
                Title = newMovie.Title,
                Rating = newMovie.Rating,
                ReleaseDate = newMovie.ReleaseDate,
                Description = newMovie.Description,
                Director = director
            };

            _context.Movie.Add(finalMovieToAdd);
            await _context.SaveChangesAsync();

            var movieDto = new MovieDto(
                finalMovieToAdd.Id,
                finalMovieToAdd.Title,
                finalMovieToAdd.Rating,
                finalMovieToAdd.ReleaseDate,
                finalMovieToAdd.Description,
                finalMovieToAdd.Director.Name
                );


            return CreatedAtAction(nameof(GetMovie), new { id = finalMovieToAdd.Id }, movieDto);
        }

        [HttpPut("Movies/{movieId}")]
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
            await _context.SaveChangesAsync();

            return NoContent();

        }


        [HttpDelete("Movies/{id}")]
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

        [HttpGet("Movies/{id}/details")]
        public async Task<ActionResult<IEnumerable<MovieDetailDto>>> GetMovieDetails(int id)
        {
            var dto = await _context.Movie
                .Include(m => m.Director.ContactInfo)
                .Where(m => m.Id == id)
                .Select(m => new MovieDetailDto
                (
                    m.Id,
                    m.Title,
                    m.Rating,
                    m.ReleaseDate,
                    m.Description,
                    m.Genres.Select(g => g.GenreName).ToList(),
                    m.Actors.Select(a => a.Name).ToList(),
                    new DirectorDto
                    (
                        m.Director.Name,
                        m.Director.DateOfBirth,
                        new ContactInfoDto(m.Director.ContactInfo.Email, m.Director.ContactInfo.PhoneNr)
                    )
                ))
                .FirstOrDefaultAsync();

            if (dto == null)
            {
                return NotFound();
            }

            return Ok(dto);
        }



    }
}
