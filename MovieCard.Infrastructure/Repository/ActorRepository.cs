using MovieCard.Models.Entities;
using MovieCard.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MovieCard.Shared.DTOs;


namespace MovieCard.Infrastructure.Repository
{
    public class ActorRepository : RepositoryBase<Actor>, IActorRepository
    {
        public ActorRepository(MovieCardContext context) : base(context) { }

        public async Task<IEnumerable<Actor>> GetActorsAsync(bool trackChanges)
        {
            return await GetAll(trackChanges).ToListAsync();
        }

        public async Task<Actor?> GetActorByIdAsync(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, false).FirstOrDefaultAsync();
        }

        public async Task<Actor?> GetActorByNameAsync(string name)
        {
            var actorExists = await FindByCondition(a => a.Name.Equals(name), false).FirstOrDefaultAsync();
            if (actorExists != null)
            {
                return actorExists;
            }
            return null;
        }

        public async Task<bool> CreateNewActor(ActorForCreationDto newActor)
        {
            if (newActor.Name == null || newActor.DateOfBirth == null)
            {
                return false;
            }

            var actor = new Actor()
            {
                Name = newActor.Name,
                DateOfBirth = newActor.DateOfBirth,
            };

            await CreateAsync(actor);
            await Context.SaveChangesAsync();

            return true;
        }
    }
}
