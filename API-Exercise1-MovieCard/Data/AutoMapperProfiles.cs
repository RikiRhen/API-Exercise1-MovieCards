using API_Exercise1_MovieCard.Models.DTOs;
using API_Exercise1_MovieCard.Models.Entities;
using AutoMapper;

namespace API_Exercise1_MovieCard.Data
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Movie, MovieDto>()
                .ConstructUsing(src => new MovieDto(src.Id, src.Title, src.Rating, src.ReleaseDate, src.Description, src.Director.Name));
            CreateMap<MovieForCreationDto, Movie>().ReverseMap();
            CreateMap<MovieForUpdateDto, Movie>().ReverseMap();
            CreateMap<Movie, MovieDetailDto>()
                .ConstructUsing(src => new MovieDetailDto(
                    src.Id, 
                    src.Title, 
                    src.Rating, 
                    src.ReleaseDate, 
                    src.Description, 
                    src.Genres.Select(g => g.GenreName).ToList(), 
                    src.Actors.Select(a => a.Name).ToList(), 
                    new DirectorDto(src.Director.Name, src.Director.DateOfBirth, new ContactInfoDto(src.Director.ContactInfo.Email, src.Director.ContactInfo.PhoneNr))))
                .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.Director));

            CreateMap<ContactInformation, ContactInfoDto>()
                .ConstructUsing(src => new ContactInfoDto(src.Email, src.PhoneNr));

            CreateMap<Actor, ActorDto>().ReverseMap();

            CreateMap<Director, DirectorDto>()
                .ConstructUsing(src => new DirectorDto(
                    src.Name,
                    src.DateOfBirth,
                    new ContactInfoDto(
                        src.ContactInfo.Email,
                        src.ContactInfo.PhoneNr
                     )));
        }
    }
}
