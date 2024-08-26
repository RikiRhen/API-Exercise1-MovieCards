namespace API_Exercise1_MovieCard.Models.Entities
{
    public class Genre
    {
        //Id, Genre name
        public int Id { get; set; }
        public string GenreName { get; set; }

        //Navigation
        //Relationships -> N:M Movie
        public ICollection<Movie> Movies { get; set; }
    }
}
