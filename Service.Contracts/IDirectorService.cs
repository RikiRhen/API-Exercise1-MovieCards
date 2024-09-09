using MovieCard.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IDirectorService
    {
        Task<IEnumerable<DirectorDto>> GetDirectorsAsync(bool trackChanges);
        Task<DirectorDto> GetDirectorDtoByIdAsync(int id, bool trackChanges);
    }
}
