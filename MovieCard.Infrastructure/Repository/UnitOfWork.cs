using Microsoft.EntityFrameworkCore;
using MovieCard.Contracts;
using MovieCard.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCard.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MovieCardContext _context;
        private readonly Lazy<IActorRepository> _actorRepository;
        private readonly Lazy<IDirectorRepository> _directorRepostiory;
        private readonly Lazy<IMovieRepository> _movieRepository;

        public IActorRepository Actors => _actorRepository.Value;
        public IDirectorRepository Directors => _directorRepostiory.Value;
        public IMovieRepository Movies => _movieRepository.Value;

        public UnitOfWork(MovieCardContext context, Lazy<IActorRepository> actorRepository, Lazy<IDirectorRepository> directorRepository, Lazy<IMovieRepository> movieRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _actorRepository = actorRepository;
            _directorRepostiory = directorRepository;
            _movieRepository = movieRepository;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
