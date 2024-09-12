using MovieCard.Models.Entities;
using MovieCard.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


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
    }
}
