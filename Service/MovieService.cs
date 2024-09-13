using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;
using MovieCard.Contracts;
using MovieCard.Models.Entities;
using MovieCard.Shared.DTOs;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Service
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public MovieService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        //OBJECT MARKERAD
        public async Task<IEnumerable<object>> GetMoviesAsync(string? title, string? genre, string? director, string? actor, string? releaseDate, string? sortBy, string? sortOrder, bool trackChanges, bool detailed)
        {
            var query = await _uow.Movies.GetMoviesAsync(title, genre, director, actor, releaseDate, sortBy, sortOrder, trackChanges, detailed, _mapper);
            return query;
            //return _mapper.Map<IEnumerable<MovieDto>>(await _uow.Movies.GetMoviesAsync(trackChanges));
        }

        public async Task<MovieDto> GetMovieDtoByIdAsync(int id, bool trackChanges = false)
        {
            var dto = await _uow.Movies.GetMovieByIdAsync(id, trackChanges);
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }
            return _mapper.Map<MovieDto>(dto);
        }

        public async Task<MovieDto?> GetMovieDtoByTitleAsync(string title, bool trackChanges = false)
        {
            var dto = await _uow.Movies.GetMovieByTitleAsync(title, trackChanges);
            if (dto != null)
            {
                return _mapper.Map<MovieDto>(dto);
            }
            else
            {
                return null;
            }
        }
        public async Task<MovieDto?> CreateNewMovieAsync(MovieForCreationDto newMovie)
        {
            var movieToBeCreated = await _uow.Movies.CreateNewMovieAsync(newMovie);
            var id = movieToBeCreated.Id;
            movieToBeCreated = await _uow.Movies.GetMovieByIdAsync(id, false);
            if (movieToBeCreated == null) { return null; }

            var dtoToReturn = _mapper.Map<MovieDto>(movieToBeCreated);
            return dtoToReturn;
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            var success = await _uow.Movies.DeleteMovieAsync(id);
            return success;
        }
    }
}
