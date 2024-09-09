using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IServiceManager
    {
        IActorService ActorService { get; }
        IDirectorService DirectorService { get; }
        IMovieService MovieService { get; }
    }
}
