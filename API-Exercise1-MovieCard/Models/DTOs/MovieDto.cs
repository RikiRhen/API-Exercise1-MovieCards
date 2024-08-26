using API_Exercise1_MovieCard.Models.Entities;

namespace API_Exercise1_MovieCard.Models.DTOs
{
    public record MovieDto(int Id, string Title, int Rating, string ReleaseDate, string Description, ICollection<string> Genres, ICollection<string> Actors, string Director);

    public class MovieByIdDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
        public string ReleaseDate { get; set; }
        public string Description { get; set; }
        public ICollection<string> Genres { get; set; }
        public ICollection<string> Actors { get; set; }
        public string Director { get; set; }
    }
}
