using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCard.Contracts
{
    public interface IUnitOfWork
    {
        IActorRepository Actors { get; }
        IDirectorRepository Directors { get; }
        IMovieRepository Movies { get; }

        Task CompleteAsync();
    }
}
