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
        [HttpGet("Movies")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var dto = await _context.Movie.Include(m => m.Director).ProjectTo<MovieDto>(_mapper.ConfigurationProvider).ToListAsync();

            return Ok(dto);
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

            var finalMovieToAdd = _mapper.Map<Movie>(newMovie);
            finalMovieToAdd.Director = director;

            _context.Movie.Add(finalMovieToAdd);
            await _context.SaveChangesAsync();

            var movieDto = _mapper.Map<MovieDto>(finalMovieToAdd);


            return CreatedAtAction(nameof(GetMovie), new { id = finalMovieToAdd.Id }, movieDto);
        }

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

            var movie = await _context.Movie
                .Include(m => m.Director)
                .Include(m => m.Director.ContactInfo)
                .Include(m => m.Actors)
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<MovieDetailDto>(movie);

            if (dto == null)
            {
                return NotFound();
            }

            return Ok(dto);
        }

    }
}
