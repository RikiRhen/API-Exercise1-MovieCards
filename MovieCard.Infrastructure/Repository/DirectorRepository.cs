using Microsoft.EntityFrameworkCore;
using MovieCard.Contracts;
using MovieCard.Infrastructure.Data;
using MovieCard.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCard.Infrastructure.Repository
{
    public class DirectorRepository : RepositoryBase<Director>, IDirectorRepository
    {
        public DirectorRepository(MovieCardContext context) : base(context) { }

        public async Task<IEnumerable<Director>> GetDirectorsAsync(bool trackChanges)
        {
            return await GetAll(trackChanges).ToListAsync();
        }

        public async Task<Director?> GetDirectorByIdAsync(int id, bool trackChanges)
        {
            return await FindByCondition(d => d.Id.Equals(id), trackChanges).FirstOrDefaultAsync();
        }

        public async Task<Director?> GetDirectorByNameAsync(string name, bool trackChanges)
        {
            return await FindByCondition(d => d.Name.Replace(" ", "").ToLower().Equals(name.ToLower()), trackChanges).FirstOrDefaultAsync();
        }
    }
}
