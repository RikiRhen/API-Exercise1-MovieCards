using AutoMapper;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MovieCard.Contracts;
using MovieCard.Shared.DTOs;
using Service.Contracts;

namespace Service
{
    public class ActorService : IActorService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ActorService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActorDto>> GetActorsAsync(bool trackChanges = false)
        {
            return _mapper.Map<IEnumerable<ActorDto>>(await _uow.Actors.GetActorsAsync(trackChanges));
        }

        public async Task<ActorDto> GetActorDtoByIdAsync(int id, bool trackChanges = false)
        {
            var actor = await _uow.Actors.GetActorByIdAsync(id, trackChanges);
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }
            return _mapper.Map<ActorDto>(actor);
        }

        public async Task<ActorDto?> GetActorDtoByNameAsync(string name)
        {
            var actorExists = await _uow.Actors.GetActorByNameAsync(name);
            if (actorExists != null)
            {
                var actorFound =_mapper.Map<ActorDto>(actorExists);
                return actorFound;
            }
            return null;
        }

        public async Task<bool> CreateNewActor(ActorForCreationDto newActor)
        {
            var success = await _uow.Actors.CreateNewActor(newActor);
            if (success) { return true; }
            return false;
        }
    }
}
