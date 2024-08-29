﻿using Microsoft.Identity.Client;
using System.Text.Json.Serialization;

namespace API_Exercise1_MovieCard.Models.Entities
{
    public class ContactInformation
    {
        //Id, Email, phone nr
        public int Id { get; set; }
        public string Email { get; set; }
        public string PhoneNr { get; set; }
        public int DirectorId { get; set; }

        //Navigation
        [JsonIgnore]
        public Director Director { get; set; }
    }
}
