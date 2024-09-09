using MovieCard.Models.Entities;

namespace MovieCard.Contracts
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetMoviesAsync(bool trackChanges);
        Task<Movie?> GetMovieByIdAsync(int id, bool trackChanges);
    }
}
