
namespace MovieCard.Shared.DTOs
{
    public record MovieDetailDto (int Id, string Title, int Rating, string ReleaseDate, string Description, ICollection<string> GenreNames, ICollection<string> ActorNames, DirectorDto Director);
}
