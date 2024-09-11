using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieCard.API.Extensions;
using MovieCard.Infrastructure.Data;
using MovieCard.Infrastructure.Repository;
using MovieCard.Contracts;
using Service;
using Service.Contracts;
using System.Reflection.Metadata;
using System.Net;
using MovieCard.Presentation;

namespace API_Exercise1_MovieCard
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddDbContext<MovieCardContext>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieCardContext") ?? throw new InvalidOperationException("Connection string 'MovieCardContext' not found.")));
            builder.Services.ConfigureSql(builder.Configuration);

            builder.Services.AddControllers(configure => configure.ReturnHttpNotAcceptable = true)
                            .AddNewtonsoftJson()
                            .AddApplicationPart(typeof(AssemblyRef).Assembly);

            builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
            //builder.Services.AddEndpointsApiExplorer();
            builder.Services.ConfigureOpenApi();
            builder.Services.ConfigureServices();
            builder.Services.ConfigureRepositories();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                await app.SeedDataAsync();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
