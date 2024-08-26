﻿using API_Exercise1_MovieCard.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace API_Exercise1_MovieCard.Extensions
{
    public static class WebbapplicationsExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var context = serviceProvider.GetRequiredService<MovieCardContext>();

                await context.Database.EnsureDeletedAsync();
                await context.Database.MigrateAsync();

                try
                {
                    await SeedData.InitializeAsync(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
    }
}
