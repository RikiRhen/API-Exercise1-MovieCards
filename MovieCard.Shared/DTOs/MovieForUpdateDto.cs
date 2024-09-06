
namespace MovieCard.Shared.DTOs
{
    public class MovieForUpdateDto
    {
        public string Title { get; set; }
        public int Rating { get; set; }
        public string ReleaseDate { get; set; }
        public string Description { get; set; }
        public int DirectorId { get; set; }
    }
}
