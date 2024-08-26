using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using API_Exercise1_MovieCard.Data;
using API_Exercise1_MovieCard.Extensions;

namespace API_Exercise1_MovieCard
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MovieCardContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MovieCardContext") ?? throw new InvalidOperationException("Connection string 'MovieCardContext' not found.")));


            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
