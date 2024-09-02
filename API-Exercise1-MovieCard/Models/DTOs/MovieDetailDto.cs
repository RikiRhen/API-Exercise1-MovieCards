using API_Exercise1_MovieCard.Models.Entities;

namespace API_Exercise1_MovieCard.Models.DTOs
{
    public record MovieDetailDto(int Id, string Title, int Rating, string ReleaseDate, string Description, ICollection<string> GenreNames, ICollection<string> ActorNames, DirectorDto Director);
}
