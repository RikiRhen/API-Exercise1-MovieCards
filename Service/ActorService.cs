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

        public async Task<ActorDto> GetActorDtoByNameAsync(string name, bool trackChanges = false)
        {
            var dto = await _uow.Actors.GetActorByNameAsync(name, trackChanges);
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }
            return _mapper.Map<ActorDto>(dto);
        }
    }
}
