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
        Task<IEnumerable<MovieDto>> GetMoviesAsync(bool trackChanges);
        Task<MovieDto> GetMovieDtoByIdAsync(int id, bool trackChanges);
    }
}
