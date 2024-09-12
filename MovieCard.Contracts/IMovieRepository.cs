using Microsoft.EntityFrameworkCore.Metadata;
using MovieCard.Models.Entities;
using AutoMapper;
using MovieCard.Shared.DTOs;

namespace MovieCard.Contracts
{
    public interface IMovieRepository
    {
        //OBJECT MARKERAD
        Task<IEnumerable<object>> GetMoviesAsync(string? title, string? genre, string? director, string? actor, string? releaseDate, string? sortBy, string? sortOrder, bool trackChanges, bool detailed, IMapper mapper);
        Task<Movie?> GetMovieByIdAsync(int id, bool trackChanges);
        Task<Movie?> GetMovieByTitleAsync(string title, bool trackChanges);
        Task<Movie> CreateNewMovieAsync(MovieForCreationDto movie);
    }
}
