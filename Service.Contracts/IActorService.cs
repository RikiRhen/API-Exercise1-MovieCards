using MovieCard.Shared.DTOs;

namespace Service.Contracts
{
    public interface IActorService
    {
        Task<IEnumerable<ActorDto>> GetActorsAsync(bool trackChanges);
        Task<ActorDto> GetActorDtoByNameAsync(string name, bool trackChanges);
    }
}
