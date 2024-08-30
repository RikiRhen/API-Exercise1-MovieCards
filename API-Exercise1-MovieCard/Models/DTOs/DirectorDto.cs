using API_Exercise1_MovieCard.Models.Entities;

namespace API_Exercise1_MovieCard.Models.DTOs
{
    public record DirectorDto(string Name, string DateOfBirth, ContactInfoDto ContactInfo);
}
