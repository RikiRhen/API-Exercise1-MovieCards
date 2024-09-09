using Microsoft.EntityFrameworkCore;
using MovieCard.Contracts;
using MovieCard.Infrastructure.Data;
using MovieCard.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MovieCard.Infrastructure.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected MovieCardContext Context { get; }
        protected DbSet<T> DbSet { get; }

        public RepositoryBase(MovieCardContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public IQueryable<T> GetAll(bool trackChanges) => 
            !trackChanges ? DbSet.AsNoTracking() : DbSet;

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ? DbSet.Where(expression).AsNoTracking() :
                            DbSet.Where(expression);

        public void Update (T entity) => DbSet.Update(entity);
        public async Task CreateAsync(T entity) => await DbSet.AddAsync(entity);
        public void Delete(T entity) => DbSet.Remove(entity);
    }
}
