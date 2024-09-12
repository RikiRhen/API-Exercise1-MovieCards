using MovieCard.Models.Entities;
using MovieCard.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IMovieService
    {
        //OBJECT MARKERAD
        Task<IEnumerable<object>> GetMoviesAsync(string? title, string? genre, string? director, string? actor, string? releaseDate, string? sortBy, string? sortOrder, bool trackChanges, bool detailed);
        Task<MovieDto> GetMovieDtoByIdAsync(int id, bool trackChanges);
        Task<MovieDto?> GetMovieDtoByTitleAsync(string title, bool trackChanges);
        Task<MovieDto?> CreateNewMovieAsync(MovieForCreationDto newMovie);
    }
}
