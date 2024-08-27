using API_Exercise1_MovieCard.Models.Entities;

namespace API_Exercise1_MovieCard.Models.DTOs
{
    public record MovieDto(int Id, string Title, int Rating, string ReleaseDate, string Description, string Director);
}
