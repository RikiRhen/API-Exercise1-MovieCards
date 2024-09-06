using MovieCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
    }
}
