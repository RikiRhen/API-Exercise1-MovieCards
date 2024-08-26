namespace API_Exercise1_MovieCard.Models.Entities
{
    public class Movie
    {
        //Id, title, rating, release date, description,

        public int Id { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
        public string ReleaseDate { get; set; }
        public string Description { get; set; }
        public int DirectorId { get; set; }

        //Relationships -> 1:M Director, N:M Actor, N:M Genre
        //Navigation
        public Director Director { get; set; }
        public ICollection<Actor> Actors { get; set; } = new List<Actor>();
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();

    }
    
}
