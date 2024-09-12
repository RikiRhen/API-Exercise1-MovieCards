using MovieCard.Shared.DTOs;

namespace Service.Contracts
{
    public interface IActorService
    {
        Task<IEnumerable<ActorDto>> GetActorsAsync(bool trackChanges);
        Task<ActorDto> GetActorDtoByIdAsync(int id, bool trackChanges);
    }
}
