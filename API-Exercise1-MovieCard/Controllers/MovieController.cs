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
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace API_Exercise1_MovieCard.Controllers
{
    [Route("api")]
    [ApiController]
    [Produces("application/json")]
    public class MovieController : ControllerBase
    {
        private readonly MovieCardContext _context;
        private readonly IMapper _mapper;

        public MovieController(MovieCardContext context, IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
        }
        
        // GET MOVIES
        //GET: api/Movies
        //[HttpGet("Movies")]
        //public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        //{
        //    var dto = await _context.Movie.Include(m => m.Director).ProjectTo<MovieDto>(_mapper.ConfigurationProvider).ToListAsync();

        //    return Ok(dto);
        //}

        [HttpGet("Movies")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies(string? title,string? genre, string? director)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                title = title.Trim();
                var dtoTitle = await _context.Movie
                    .Where(m => m.Title == title)
                    .Include(m => m.Director)
                    .ProjectTo<MovieDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                return Ok(dtoTitle);
            } 
            else if (!string.IsNullOrWhiteSpace(genre))
            {
                genre = genre.Trim();
                var dtoGenre = await _context.Movie
                    .Where(m => m.Genres.Any(g => g.GenreName == genre))
                    .Include(m => m.Director)
                    .ProjectTo<MovieDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                return Ok(dtoGenre);
            }
            else if (!string.IsNullOrWhiteSpace(director))
            {
                director = director.Trim();
                var dtoDirector = await _context.Movie.Where(m => m.Director.Name == director).Include(m => m.Director).ProjectTo<MovieDto>(_mapper.ConfigurationProvider).ToListAsync();
                return Ok(dtoDirector);
            }

            var dtoBlank = await _context.Movie.Include(m => m.Director).ProjectTo<MovieDto>(_mapper.ConfigurationProvider).ToListAsync();
            return Ok(dtoBlank);

        }

        // GET ACTORS
        //GET: api/Actors
        [HttpGet("Actors")]
        public async Task<ActionResult<IEnumerable<ActorDto>>> GetActors()
        {
            var dto = await _context.Actor.ProjectTo<ActorDto>(_mapper.ConfigurationProvider).ToListAsync();
            return Ok(dto);
        }

        //GET DIRECTORS
        //GET: api/Directors
        [HttpGet("Directors")]
        public async Task<ActionResult<IEnumerable<DirectorDto>>> GetDirectors()
        {
            var dto = await _context.Director.ProjectTo<DirectorDto>(_mapper.ConfigurationProvider).ToListAsync();
            return Ok(dto);
        }

        //GET MOVIE BY ID
        //GET: api/Movies/1
        [HttpGet("Movies/{id}", Name = "GetMovie")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {

            var dto = _mapper.Map<MovieDto>(await _context.Movie.Include(m => m.Director).FirstOrDefaultAsync(m => m.Id == id));

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

            var dto = _mapper.Map<ActorDto>(await _context.Actor.FirstOrDefaultAsync(a => a.Id == id));

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

            var dto = _mapper.Map<DirectorDto>(await _context.Director.FirstOrDefaultAsync(d => d.Id == id));

            if (dto == null)
            {
                return NotFound();
            }

            return Ok(dto);
        }

        //CREATE NEW MOVIE
        //POST: api/Movies
        [HttpPost("Movies")]
        public async Task<ActionResult<Movie>> CreateMovie(MovieForCreationDto newMovie)
        {
            if (newMovie == null)
            {
                return BadRequest("A body that results in a null object was sent with request.");
            }

            if (DateTime.TryParse(newMovie.ReleaseDate, out DateTime parsedDate))
            {
                if (parsedDate > DateTime.Now)
                {
                    return BadRequest("Release date of movie cannot be set in the future");
                }
            }

            var movieExists = await _context.Movie.FirstOrDefaultAsync(m => m.Title == newMovie.Title);
            if (movieExists != null)
            {
                return BadRequest("A movie with that title already exists");
            }

            if (newMovie.Rating < 1 || newMovie.Rating > 10)
            {
                return BadRequest("Rating of a movie should be between 1-10");
            }

            var director = await _context.Director.FindAsync(newMovie.DirectorId);
            if (director == null)
            {
                return NotFound($"Director with ID {newMovie.DirectorId} was not found");
            }

            var finalMovieToAdd = _mapper.Map<Movie>(newMovie);

            _context.Movie.Add(finalMovieToAdd);
            await _context.SaveChangesAsync();

            var movieDto = _mapper.Map<MovieDto>(finalMovieToAdd);


            return CreatedAtAction(nameof(GetMovie), new { id = finalMovieToAdd.Id }, movieDto);
        }

        //CREATE NEW ACTOR
        //POST: api/Actors
        [HttpPost("Actors")]
        public async Task<ActionResult<Actor>> CreateActor(ActorForCreationDto newActor)
        {
            if (newActor == null)
            {
                return BadRequest("A body that results in a null object was sent with request.");
            }

            var actorExists = await _context.Actor.FirstOrDefaultAsync(a => a.Name == newActor.Name && a.DateOfBirth == newActor.DateOfBirth);
            if (actorExists != null)
            {
                return BadRequest("An actor with that information already exists. Duplicates are not allowed");
            }

            var newActorToAdd = _mapper.Map<Actor>(newActor);

            _context.Actor.Add(newActorToAdd);
            await _context.SaveChangesAsync();

            var actorDto = _mapper.Map<ActorDto>(newActorToAdd);

            return CreatedAtAction(nameof(GetActor), new { id = newActorToAdd.Id }, actorDto);
        }

        //REPLACE EXISTING MOVIE
        //PUT: api/Movies/5
        [HttpPut("Movies/{movieId}")]
        public async Task<ActionResult> UpdateMovie(int movieId, MovieForUpdateDto updateMovie)
        {
            var movie = await _context.Movie.Include(m => m.Director).Include(m => m.Actors).Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == movieId);
            if (movie == null)
            {
                return NotFound();
            }

            _mapper.Map(updateMovie, movie);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        //DELETE EXISTING MOVIE
        //DELETE: api/Movies/5
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

        //GET DETAILED INFORMATION ABOUT MOVIE
        //GET: api/Movies/5/details
        [HttpGet("Movies/{id}/details")]
        public async Task<ActionResult<IEnumerable<MovieDetailDto>>> GetMovieDetails(int id)
        {

            var dto = await _context.Movie
                .Include(m => m.Director)
                .Include(m => m.Director.ContactInfo)
                .Include(m => m.Actors)
                .Include(m => m.Genres)
                .ProjectTo<MovieDetailDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (dto == null)
            {
                return NotFound();
            }

            return Ok(dto);
        }

    }
}
