using MovieCard.Models.Entities;
using MovieCard.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCard.Contracts
{
    public interface IActorRepository
    {
        Task<IEnumerable<Actor>> GetActorsAsync(bool trackChanges);
        Task<Actor?> GetActorByIdAsync(int id, bool trackChanges);
        Task<Actor?> GetActorByNameAsync(string name);
        Task<bool> CreateNewActor(ActorForCreationDto newActor);
    }
}
