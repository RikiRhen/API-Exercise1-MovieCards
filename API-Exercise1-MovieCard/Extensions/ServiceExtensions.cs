using MovieCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MovieCard.Contracts;
using MovieCard.Infrastructure.Repository;
using Service;
using Service.Contracts;

namespace MovieCard.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSql(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MovieCardContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("MovieCardContext") ?? throw new InvalidOperationException("Connection string 'MovieCardContext' not found.")));
        }

        public static void ConfigureOpenApi(this IServiceCollection services) => services.AddEndpointsApiExplorer().AddSwaggerGen();

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IActorService,  ActorService>();
            services.AddScoped<IDirectorService, DirectorService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped(provider => new Lazy<IActorService>(() => provider.GetRequiredService<IActorService>()));
            services.AddScoped(provider => new Lazy<IDirectorService>(() => provider.GetRequiredService<IDirectorService>()));
            services.AddScoped(provider => new Lazy<IMovieService>(() => provider.GetRequiredService<IMovieService>()));
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IActorRepository, ActorRepository>();
            services.AddScoped<IDirectorRepository, DirectorRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped(provider => new Lazy<IActorRepository>(() => provider.GetRequiredService<IActorRepository>()));
            services.AddScoped(provider => new Lazy<IDirectorRepository>(() => provider.GetRequiredService<IDirectorRepository>()));
            services.AddScoped(provider => new Lazy<IMovieRepository>(() => provider.GetRequiredService<IMovieRepository>()));
        }
    }
}
