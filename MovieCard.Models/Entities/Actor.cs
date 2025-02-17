﻿using System.Text.Json.Serialization;

namespace MovieCard.Models.Entities
{
    public class Actor
    {
        //Id, name, date of birth
        public int Id { get; set; }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }

        //Relationships -> N:M Movie
        //Navigation
        [JsonIgnore]
        public ICollection<Movie> Movies { get; set; }

    }
}
