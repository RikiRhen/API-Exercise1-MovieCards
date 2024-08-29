
namespace API_Exercise1_MovieCard.Models.Entities
{
    public class Director
    {
        //Id, name, date of birth

        public int Id { get; set; }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }

        //Relationship -> 1:1 Contactinfo
        //Navigation
        public ContactInformation ContactInfo {  get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
