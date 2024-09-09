using MovieCard.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCard.Contracts
{
    public interface IDirectorRepository
    {
        Task<IEnumerable<Director>> GetDirectorsAsync(bool trackChanges);
        Task<Director?> GetDirectorByIdAsync(int id, bool trackChanges);
        Task<Director?> GetDirectorByNameAsync(string name, bool trackChanges);
    }
}
