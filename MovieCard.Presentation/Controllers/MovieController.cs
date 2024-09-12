using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.JsonPatch;
using MovieCard.Infrastructure.Data;
using MovieCard.Shared.DTOs;
using MovieCard.Models.Entities;
using Service;

namespace MovieCard.Presentation.Controllers
{
    [Route("api")]
    [ApiController]
    [Produces("application/json")]
    public class MovieController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public MovieController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        
        // GET MOVIES
        //GET: api/Movies
        [HttpGet("Movies")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies(/*[FromQuery] MovieParams movieparams*/ string? title, string? genre, string? director, string? actor, string? releaseDate, string? sortBy, string? sortOrder, bool detailed = false, bool trackChanges = false)
        {
            var query = await _serviceManager.MovieService.GetMoviesAsync(title, genre, director, actor, releaseDate, sortBy, sortOrder, trackChanges, detailed);
            return Ok(query);

        }

        // GET ACTORS
        //GET: api/Actors
        [HttpGet("Actors")]
        public async Task<ActionResult<IEnumerable<ActorDto>>> GetActors()
        {
            //var dto = await _serviceManager.ActorService.ProjectTo<ActorDto>(_mapper.ConfigurationProvider).ToListAsync();
            //return Ok(dto);
            throw new NotImplementedException();
        }

        //GET DIRECTORS
        //GET: api/Directors
        [HttpGet("Directors")]
        public async Task<ActionResult<IEnumerable<DirectorDto>>> GetDirectors()
        {
            //var dto = await _serviceManager.Director.ProjectTo<DirectorDto>(_mapper.ConfigurationProvider).ToListAsync();
            //return Ok(dto);
            throw new NotImplementedException();
        }

        //GET MOVIE BY ID
        //GET: api/Movies/1
        [HttpGet("Movies/{id}", Name = "GetMovie")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var dto = await _serviceManager.MovieService.GetMovieDtoByIdAsync(id, false);

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

            //var dto = _mapper.Map<ActorDto>(await _serviceManager.Actor.FirstOrDefaultAsync(a => a.Id == id));

            //if (dto == null)
            //{
            //    return NotFound();
            //}

            //return Ok(dto);
            throw new NotImplementedException();
        }

        //GET DIRECTOR BY ID
        //GET: api/Directors/1
        [HttpGet("Directors/{id}", Name = "GetDirector")]
        public async Task<ActionResult<DirectorDto>> GetDirector(int id)
        {

            //var dto = _mapper.Map<DirectorDto>(await _serviceManager.Director.FirstOrDefaultAsync(d => d.Id == id));

            //if (dto == null)
            //{
            //    return NotFound();
            //}

            //return Ok(dto);
            throw new NotImplementedException();
        }

        //CREATE NEW MOVIE
        //POST: api/Movies
        [HttpPost("Movies")]
        public async Task<ActionResult<Movie>> CreateMovie(MovieForCreationDto newMovie)
        {
            var movie = await _serviceManager.MovieService.CreateNewMovieAsync(newMovie);
            if (movie == null)
            {
                return BadRequest("The return body of the function call is null");
            }

            if (DateTime.TryParse(newMovie.ReleaseDate, out DateTime parsedDate))
            {
                if (parsedDate > DateTime.Now)
                {
                    return BadRequest("The release date of a new movie cannot be in the future");
                }
            }

            //var movieExists = await FindByCondition(m => m.Title.Equals(newMovie.Title), false).FirstOrDefaultAsync();
            //if (movieExists != null)
            //{
            //    return movieExists;
            //}

            if (newMovie.Rating < 1 || newMovie.Rating > 10)
            {
                return BadRequest("Rating of a movie should be between 1-10");
            }

            //var director = await _serviceManager.Director.FindAsync(newMovie.DirectorId);
            //if (director == null)
            //{
            //    return NotFound($"Director with ID {newMovie.DirectorId} was not found");
            //}

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);  
        }

        //CREATE NEW ACTOR
        //POST: api/Actors
        [HttpPost("Actors")]
        public async Task<ActionResult<Actor>> CreateActor(ActorForCreationDto newActor)
        {
            //if (newActor == null)
            //{
            //    return BadRequest("A body that results in a null object was sent with request.");
            //}

            //var actorExists = await _serviceManager.Actor.FirstOrDefaultAsync(a => a.Name == newActor.Name && a.DateOfBirth == newActor.DateOfBirth);
            //if (actorExists != null)
            //{
            //    return BadRequest("An actor with that information already exists. Duplicates are not allowed");
            //}

            //var newActorToAdd = _mapper.Map<Actor>(newActor);

            //_serviceManager.Actor.Add(newActorToAdd);
            //await _serviceManager.SaveChangesAsync();

            //var actorDto = _mapper.Map<ActorDto>(newActorToAdd);

            //return CreatedAtAction(nameof(GetActor), new { id = newActorToAdd.Id }, actorDto);
            throw new NotImplementedException();
        }

        //REPLACE EXISTING MOVIE
        //PUT: api/Movies/5
        [HttpPut("Movies/{id}")]
        public async Task<ActionResult> UpdateMovie(int id, MovieForUpdateDto updateMovie)
        {
            //var movie = await _serviceManager.Movie.Include(m => m.Director).Include(m => m.Actors).Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == id);
            //if (movie == null)
            //{
            //    return NotFound();
            //}

            //_mapper.Map(updateMovie, movie);
            //await _serviceManager.SaveChangesAsync();

            //return NoContent();
            throw new NotImplementedException();

        }

        //ADD ACTOR TO EXISTING MOVIE VIA DTOS
        //PUT: api/Movies/5/ActorsByDto
        [HttpPut("Movies/{id}/ActorsByDto")]
        public async Task<ActionResult> AddActorToMovieDto(int id, List<ActorDto> actors)
        {
            //var movie = await _serviceManager.Movie
            //    .Include(m => m.Actors)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (movie == null)
            //{
            //    return NotFound("No movie with that ID was found in the database");
            //}

            //if (actors.Any())
            //{
            //    foreach (var actorDto in actors)
            //    {
            //        var actor = await _serviceManager.Actor.FirstOrDefaultAsync(a => a.Name == actorDto.Name);
            //        if (actor == null)
            //        {
            //            actor = new Actor { Name = actorDto.Name, DateOfBirth = actorDto.DateOfBirth };
            //            _serviceManager.Actor.Add(actor);
            //        }
            //        if (!movie.Actors.Contains(actor))
            //        {
            //            movie.Actors.Add(actor);
            //        }
            //    }
            //}
            
            //var dto = await _serviceManager.Movie
            //    .Include(m => m.Director)
            //    .Include(m => m.Director.ContactInfo)
            //    .Include(m => m.Actors)
            //    .Include(m => m.Genres)
            //    .ProjectTo<MovieDetailDto>(_mapper.ConfigurationProvider)
            //    .FirstOrDefaultAsync(m => m.Id == id);

            //if (dto == null)
            //{
            //    return NotFound("Failed to create MovieDetailDto of updated Movie");
            //}

            //await _serviceManager.SaveChangesAsync();
            ////return NoContent();
            //return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, dto);
            throw new NotImplementedException();
        }


        //ADD ACTOR TO EXISTING MOVIE VIA IDS
        //PUT: api/Movies/5/ActorsByID
        [HttpPut("Movies/{id}/ActorsById")]
        public async Task<ActionResult> AddActorToMovieId(int id, /*List<ActorDto> addActors,*/ List<int> actorIds)
        {
            //var movie = await _serviceManager.Movie
            //    .Include(m => m.Actors)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (movie == null)
            //{
            //    return NotFound("No movie with that ID was found in the database");
            //}

            //if (actorIds.Any())
            //{
            //    foreach (var actorId in actorIds)
            //    {
            //        var actor = await _serviceManager.Actor.FirstOrDefaultAsync(m => m.Id == actorId);
            //        if (actor == null)
            //        {
            //            return NotFound($"An actor with the ID {actorId} was not found in the database");
            //        }
            //        if (!movie.Actors.Contains(actor))
            //        {
            //            movie.Actors.Add(actor);
            //        }
            //    }
            //}

            ////if (addActors.Any())
            ////{
            ////    foreach (var actorDto in addActors)
            ////    {
            ////        var actor = await _context.Actor.FirstOrDefaultAsync(a => a.Name == actorDto.Name);
            ////        if (actor == null)
            ////        {
            ////            return NotFound($"An actor with the name {actorDto.Name} was not found in the database");
            ////        }
            ////        if (!movie.Actors.Contains(actor))
            ////        {
            ////            movie.Actors.Add(actor);
            ////        }
            ////    }
            ////}

            //var dto = await _serviceManager.Movie
            //    .Include(m => m.Director)
            //    .Include(m => m.Director.ContactInfo)
            //    .Include(m => m.Actors)
            //    .Include(m => m.Genres)
            //    .ProjectTo<MovieDetailDto>(_mapper.ConfigurationProvider)
            //    .FirstOrDefaultAsync(m => m.Id == id);

            //if (dto == null)
            //{
            //    return NotFound("Failed to create MovieDetailDto of updated Movie");
            //}

            //await _serviceManager.SaveChangesAsync();
            ////return NoContent();
            //return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, dto);
            throw new NotImplementedException();
        }

        //PARTIALLY UPDATE EXISTING MOVIE
        //PATCH: api/Movies/5
        [HttpPatch("Movies/{id}")]
        public async Task<ActionResult> PatchMovie(int id, JsonPatchDocument<MovieForUpdateDto> patchDocument)
        {
            //var movie = await _serviceManager.Movie.FirstOrDefaultAsync(m => m.Id == id);
            //if (movie == null)
            //{
            //    return NotFound("A movie with that ID was not found in the database");
            //}

            //var movieToPatch = _mapper.Map<MovieForUpdateDto>(movie);
            //patchDocument.ApplyTo(movieToPatch, ModelState);

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest("Patch document caused ModelState to become invalid");
            //}

            //movie.Title = movieToPatch.Title;
            //movie.Rating = movieToPatch.Rating;
            //movie.ReleaseDate = movieToPatch.ReleaseDate;
            //movie.Description = movieToPatch.Description;
            //movie.DirectorId = movieToPatch.DirectorId;
            //await _serviceManager.SaveChangesAsync();

            //return NoContent();
            throw new NotImplementedException();
        }

        //DELETE EXISTING MOVIE
        //DELETE: api/Movies/5
        [HttpDelete("Movies/{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            //var movie = await _serviceManager.Movie.FindAsync(id);
            //if (movie == null)
            //{
            //    return NotFound();
            //}

            //_serviceManager.Movie.Remove(movie);
            //await _serviceManager.SaveChangesAsync();

            //return NoContent();
            throw new NotImplementedException();
        }

        //GET DETAILED INFORMATION ABOUT MOVIE
        //GET: api/Movies/5/details
        [HttpGet("Movies/{id}/details")]
        public async Task<ActionResult<IEnumerable<MovieDetailDto>>> GetMovieDetails(int id)
        {

            //var dto = await _serviceManager.Movie
            //    .Include(m => m.Director)
            //    .Include(m => m.Director.ContactInfo)
            //    .Include(m => m.Actors)
            //    .Include(m => m.Genres)
            //    .ProjectTo<MovieDetailDto>(_mapper.ConfigurationProvider)
            //    .FirstOrDefaultAsync(m => m.Id == id);

            //if (dto == null)
            //{
            //    return NotFound();
            //}

            //return Ok(dto);
            throw new NotImplementedException();
        }

    }
}
