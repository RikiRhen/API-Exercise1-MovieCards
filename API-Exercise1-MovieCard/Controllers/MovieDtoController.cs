using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API_Exercise1_MovieCard.Data;
using API_Exercise1_MovieCard.Models.DTOs;

namespace API_Exercise1_MovieCard.Controllers
{
    [Route("api/Movies")]
    [ApiController]
    public class MovieDtoController : ControllerBase
    {
        private readonly MovieCardContext _context;

        public MovieDtoController(MovieCardContext context)
        {
            _context = context;
        }

        //GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var dto = _context.Movie.Select(m => new MovieDto(m.Id, m.Title, m.Rating, m.ReleaseDate, m.Description, m.Genres.Select(g => g.GenreName).ToList(), m.Actors.Select(a => a.Name).ToList(), m.Director.Name));
            return Ok(await dto.ToListAsync());
        }

        //GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            throw new NotImplementedException();
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
