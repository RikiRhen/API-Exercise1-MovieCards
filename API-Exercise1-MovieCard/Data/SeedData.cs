using API_Exercise1_MovieCard.Models.Entities;
using Bogus;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace API_Exercise1_MovieCard.Data
{
    internal class SeedData
    {
        private static Faker faker = new Faker("sv");

        internal static async Task InitializeAsync(MovieCardContext context)
        {
            //Check if data exists. If so, return.
            if (await context.Movie.AnyAsync())
            {
                Console.WriteLine("Initialize SeedData cancelled, found existing data");
                return;
            }

            var actors = GenerateActors(10);
            await context.AddRangeAsync(actors);

            var directors = GenerateDirectors(4);
            await context.AddRangeAsync(directors);

            var genres = new List<Genre>
            {
                new Genre { GenreName = "Action" },
                new Genre { GenreName = "Comedy" },
                new Genre { GenreName = "Drama" },
                new Genre { GenreName = "Horror" },
                new Genre { GenreName = "Sci-Fi" }

            };
            await context.AddRangeAsync(genres);

            var movies = GenerateMovies(30, directors, actors, genres);
            await context.AddRangeAsync(movies);

            await context.SaveChangesAsync();

        }


        private static IEnumerable<Director> GenerateDirectors(int numberOfDirectors)
        {
            var directors = new Faker<Director>("sv").Rules((faker, director) =>
            {
                director.Name = faker.Name.FullName();
                director.DateOfBirth = faker.Date.PastDateOnly().ToString();
                director.ContactInfo = new ContactInformation() 
                { 
                    Email = director.Name.Replace(" ", "") + "@mail.me", 
                    PhoneNr = faker.Phone.PhoneNumber() 
                };
            });

            return directors.Generate(numberOfDirectors);

        }

        private static IEnumerable<Actor> GenerateActors(int numberOfActors)
        {
            var actors = new Faker<Actor>("sv").Rules((faker, actor) =>
            {
                actor.Name = faker.Name.FullName();
                actor.DateOfBirth = faker.Date.PastDateOnly().ToString();
            });

            return actors.Generate(numberOfActors);
        }

        private static IEnumerable<Movie> GenerateMovies(int numberOfMovies, IEnumerable<Director> directors, IEnumerable<Actor> actors, IEnumerable<Genre> genres)
        {
            //Generating with Faker.Generate().
            var movies = new Faker<Movie>("sv").Rules((faker, movie) =>
            {
                movie.Title = faker.Random.Word();
                movie.Rating = faker.Random.Int(1, 10);
                movie.ReleaseDate = faker.Date.PastDateOnly().ToString();
                movie.Description = faker.Lorem.Lines(2);
                movie.Director = faker.PickRandom(directors);
                int numberOfActors = faker.Random.Int(2, 6);

                //Add random ammount of Actors, 2-6.
                for (int i = 0; i < numberOfActors; i++)
                {
                    Actor addActor = faker.PickRandom(actors);
                    if (movie.Actors.Contains(addActor))
                    {
                        addActor = faker.PickRandom(actors);
                    }
                    movie.Actors.Add(addActor);
                }

                //Add random ammount of Genres, 1-3.
                int numberOfGenres = faker.Random.Int(1, 3);
                for (int i = 0;i < numberOfGenres; i++)
                {
                    Genre addGenre = faker.PickRandom(genres);
                    if (movie.Genres.Contains(addGenre))
                    {
                        addGenre = faker.PickRandom(genres);
                    }
                    movie.Genres.Add(addGenre);
                }
                
            });

            return movies.Generate(numberOfMovies);

        }
    }
}
