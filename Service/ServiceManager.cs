using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IActorService> _actorService;
        private readonly Lazy<IDirectorService> _directorService;
        private readonly Lazy<IMovieService> _movieService;

        public IActorService ActorService => _actorService.Value;
        public IDirectorService DirectorService => _directorService.Value;
        public IMovieService MovieService => _movieService.Value;

        public ServiceManager(Lazy<IActorService> actorService, Lazy<IDirectorService> directorService, Lazy<IMovieService> movieService)
        {
            _actorService = actorService;
            _directorService = directorService;
            _movieService = movieService;
        }
    }
}
